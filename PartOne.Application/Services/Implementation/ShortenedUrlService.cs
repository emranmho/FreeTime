using System.Text;
using PartOne.Application.Common;
using PartOne.Application.Common.Interfaces;
using PartOne.Application.Services.Interfaces;
using PartOne.Domain.Entities;

namespace PartOne.Application.Services.Implementation;

public class ShortenedUrlService : IShortenedUrlService
{
    private readonly IUnitOfWork _unitOfWork;

    public ShortenedUrlService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    
    
    public async Task<ResponseClass<string>> ShortenUrl(string longUrl, string? customUrl)
    {
        if (string.IsNullOrWhiteSpace(longUrl))
        {
            return new ResponseClass<string> { Success = false, Message = "Please provide a URL to shorten." };
        }

        if (!string.IsNullOrWhiteSpace(customUrl) && (customUrl.Length > 10 || customUrl.Length < 5))
        {
            return new ResponseClass<string>
            {
                Success = false,
                Message = customUrl.Length > 10 ? "Custom must not be greater than 10 characters." : "Custom must be at least 5 characters."
            };
        }

        if (!string.IsNullOrWhiteSpace(customUrl))
        {
            var existingCustomUrl = await _unitOfWork.ShortenedUrl.Get(u => u.ShortUrl == customUrl);

            if (existingCustomUrl != null)
            {
                return new ResponseClass<string> { Success = false, Message = "Short URLs limit reached for this URL." };
            }
        }


        var existingLongUrl = await _unitOfWork.ShortenedUrl.Get(u => u.LongUrl == longUrl);
        if (existingLongUrl != null)
        {
            if(existingLongUrl.ShortUrl == customUrl)
            {
                return new ResponseClass<string> { Success = true, Message = "Url already exists in the database.", Value = existingLongUrl.ShortUrl };
            }
            return new ResponseClass<string> { Success = true, Message = "Url already exists in the database.", Value = existingLongUrl.ShortUrl };
        }

        string shortCode = customUrl ?? GenerateShortCode();


        // Create a new ShortenedUrl entity
        var shortUrlEntity = new ShortenedUrl
        {
            LongUrl = longUrl,
            ShortUrl = shortCode,
            CreatedTime = DateTime.UtcNow,
            ExpireDate = DateTime.UtcNow.AddDays(2)
        };

        await _unitOfWork.ShortenedUrl.Add(shortUrlEntity);
        await _unitOfWork.Save();

        return new ResponseClass<string>
        {
            Success = true,
            Message = "New generated URL",
            Value = shortUrlEntity.ShortUrl
        };
    }

    public async Task<string> RedirectToLongUrl(string shortUrl)
    {
        if (string.IsNullOrEmpty(shortUrl))
        {
            return ("Invalid short code.");
        }
        
        var url = await _unitOfWork.ShortenedUrl.Get(u => u.ShortUrl == shortUrl,tracked:true);
        
        if (url == null)
        {
            return ("Short code not found.");
        }
        
        url.UsageCount++;
        
        await _unitOfWork.Save();

        return (url.LongUrl);
    }

    public async Task DeleteExpireUrls()
    {
        var expireUrls = await _unitOfWork.ShortenedUrl.GetAll(u=>u.ExpireDate < DateTime.UtcNow);

        foreach (var url in expireUrls)
        {
            _unitOfWork.ShortenedUrl.Remove(url);
        }

        await _unitOfWork.Save();
    }

    private string GenerateShortCode()
    {
        const string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        const int length = 7; // Adjust length as needed

        Random random = new Random();
        StringBuilder sb = new StringBuilder();

        while (sb.Length < length)
        {
            int index = random.Next(chars.Length);
            sb.Append(chars[index]);
        }
        
        string shortCode = sb.ToString();
        
        // Check for uniqueness in the database
        while (_unitOfWork.ShortenedUrl.Any(u => u.ShortUrl == shortCode))
        {
            sb.Clear(); // Clear the StringBuilder for a new attempt
            while (sb.Length < length)
            {
                int index = random.Next(chars.Length);
                sb.Append(chars[index]);
            }
            shortCode = sb.ToString();
        }
        
        return shortCode;
    }
}