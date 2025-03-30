using Microsoft.AspNetCore.Mvc;
using SnapLink.Server.Models;
using StackExchange.Redis;

namespace SnapLink.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ShortenUrlController : ControllerBase
    {
        private readonly IDatabase _db;

        public ShortenUrlController(IDatabase db)
        {
            _db = db;
        }

        [HttpPost]
        public async Task<IActionResult> ShortenUrl([FromBody] ShortUrl data)
        {
            if (data == null || string.IsNullOrEmpty(data.OriginalUrl))
            {
                return BadRequest("Invalid URL");
            }

            string shortKey = string.IsNullOrEmpty(data.CustomAlias)
                ? UrlShortener.GenerateShortKey(data.OriginalUrl)
                : data.CustomAlias;

            if (await _db.KeyExistsAsync(shortKey))
            {
                return Conflict("This custom alias is already taken.");
            }

            await _db.StringSetAsync(shortKey, data.OriginalUrl, TimeSpan.FromDays(7));

            string shortenedUrl = $"{Request.Scheme}://{Request.Host}/{shortKey}";
            return Ok(new { ShortenedUrl = shortenedUrl });
        }

        [HttpGet("{shortKey}")]
        public async Task<IActionResult> RedirectToOriginalUrl(string shortKey)
        {
            var originalUrl = await _db.StringGetAsync(shortKey);

            if (string.IsNullOrEmpty(originalUrl))
            {
                return NotFound("The shortened URL does not exist.");
            }

            return Redirect(originalUrl);
        }
    }
}
