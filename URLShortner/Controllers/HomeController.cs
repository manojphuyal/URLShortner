using System.Linq;
using System;
using System.Web.Mvc;

namespace URLShortner.Controllers
{
    public class HomeController : Controller
    {





        private readonly UrlShortenerDbContext db;

        public HomeController()
        {
            db = new UrlShortenerDbContext();
        }

        // GET: UrlShortener/Shorten?url=<LongUrl>
        public ActionResult Shorten(string url)
        {
            if (string.IsNullOrEmpty(url))
                return Content("Invalid URL");

            var existingUrlMap = db.UrlMaps.SingleOrDefault(u => u.LongUrl == url);

            if (existingUrlMap != null)
            {
                // URL already shortened, return the existing short URL
                return Content($"Shortened URL: {Request.Url.GetLeftPart(UriPartial.Authority)}/{existingUrlMap.ShortUrlKey}");
            }

            // Generate a random short key for the URL
            var shortKey = GenerateRandomShortKey();

            // Save the URL mapping to the database
            var urlMap = new UrlMap { LongUrl = url, ShortUrlKey = shortKey };
            db.UrlMaps.Add(urlMap);
            db.SaveChanges();

            return Content($"Shortened URL: {Request.Url.GetLeftPart(UriPartial.Authority)}/{shortKey}");
        }

        private string GenerateRandomShortKey()
        {
            const string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var random = new Random();
            return new string(Enumerable.Repeat(chars, 6) // 6 characters for the short key
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }


















        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreateURLShort(string longUrl)
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }
        [HttpGet]
        public ActionResult RedirectToMainURL(string shortUrl)
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}