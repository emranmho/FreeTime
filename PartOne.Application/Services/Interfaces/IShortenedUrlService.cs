namespace PartOne.Application.Services.Interfaces;

public interface IShortenedUrlService
{
    Task<string> ShortenUrl(string longUrl);
    Task<string> RedirectToLongUrl(string shortUrl);
}