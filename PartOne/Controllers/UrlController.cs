using Microsoft.AspNetCore.Mvc;
using PartOne.Application.Services.Interfaces;

namespace PartOne.Controllers;

public class UrlController : Controller
{
    private readonly IShortenedUrlService _shortenedUrl;

    public UrlController(IShortenedUrlService shortenedUrl)
    {
        _shortenedUrl = shortenedUrl;
    }
    public IActionResult MakeTiny()
    {
        return View();
    }

    [HttpGet]
    public async Task<IActionResult> ShortenUrl(string longUrl, string? customUrl)
    {
        var shortUrl = await _shortenedUrl.ShortenUrl(longUrl, customUrl);
        
        return Json(shortUrl);
    }
    
    [HttpGet]
    public async Task<IActionResult> RedirectToLongUrl(string shortUrl)
    {
         var longUrl = await _shortenedUrl.RedirectToLongUrl(shortUrl);
        
        return Redirect(longUrl);
    }
}