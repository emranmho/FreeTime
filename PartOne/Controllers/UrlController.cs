using Microsoft.AspNetCore.Mvc;
using PartOne.Application.Services.Interfaces;
using System.Text;

namespace PartOne.Controllers;

public class UrlController : Controller
{
    private readonly IShortenedUrlService _shortenedUrl;

    public UrlController(IShortenedUrlService shortenedUrl)
    {
        _shortenedUrl = shortenedUrl;
    }

    [Route("maketiny")]
    public IActionResult MakeTiny()
    {
        string uid = "";
        if (!HttpContext.Session.TryGetValue("uid", out var uidBytes))
        {
            uid = Guid.NewGuid().ToString();
            uidBytes = Encoding.UTF8.GetBytes(uid);
            HttpContext.Session.Set("uid", uidBytes);
        }
        else
        {
            uid = Encoding.UTF8.GetString(uidBytes);
        }

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


    public IActionResult MakeTiny2()
    {
        return View();
    }
}