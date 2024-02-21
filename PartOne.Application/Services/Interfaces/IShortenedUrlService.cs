using PartOne.Application.Common;

namespace PartOne.Application.Services.Interfaces;

public interface IShortenedUrlService
{
    Task<ResponseClass<string>> ShortenUrl(string longUrl, string? customUrl);
    Task<string> RedirectToLongUrl(string shortUrl);
}