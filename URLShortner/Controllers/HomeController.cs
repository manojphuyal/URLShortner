using System.Linq;
using System;
using System.Web.Mvc;
using URLShortner.Models;

namespace URLShortner.Controllers
{
    public class HomeController : Controller
    {
        private readonly UrlShortenerDbContext db;

        public HomeController()
        {
            db = new UrlShortenerDbContext();
        }
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Shorten(string url)
        {
            if (string.IsNullOrEmpty(url) || !Uri.TryCreate(url, UriKind.Absolute, out Uri uriResult) || uriResult.Scheme != Uri.UriSchemeHttp && uriResult.Scheme != Uri.UriSchemeHttps)
            {
                return Json(new { Result = false, Message = "Invalid URL" }, JsonRequestBehavior.AllowGet);
            }

            var existingUrlMap = db.UrlMaps.SingleOrDefault(u => u.LongUrl == url);

            if (existingUrlMap != null)
            {
                return Json(new { Result = true, Message = $"{Request.Url.GetLeftPart(UriPartial.Authority)}/{existingUrlMap.ShortUrlKey}" }, JsonRequestBehavior.AllowGet);
            }
            var shortKey = GenerateRandomShortKey();
            var urlMap = new UrlMap { LongUrl = url, ShortUrlKey = shortKey };
            db.UrlMaps.Add(urlMap);
            db.SaveChanges();
            return Json(new { Result = true, Message = $"{Request.Url.GetLeftPart(UriPartial.Authority)}/{shortKey}" }, JsonRequestBehavior.AllowGet);

        }

        public ActionResult Go(string shortKey)
        {
            if (string.IsNullOrEmpty(shortKey))
            {
                return RedirectToAction("Index");
            }

            string fullUrl = Request.Url.ToString();
            string domain = GetDomainFromShortUrl(fullUrl);
            if (string.IsNullOrEmpty(domain))
                return Content("Invalid short URL format");

            var urlMap = db.UrlMaps.SingleOrDefault(u => u.ShortUrlKey == shortKey);

            if (urlMap == null)
                return Content("Short URL not found");

            return Redirect(urlMap.LongUrl);
        }

        private string GetDomainFromShortUrl(string shortUrl)
        {
            Uri uri;
            if (Uri.TryCreate(shortUrl, UriKind.Absolute, out uri))
            {
                return uri.Authority;
            }
            return null;
        }
        private string GenerateRandomShortKey()
        {
            const string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var random = new Random();
            while (true)
            {
                string shortKey = new string(Enumerable.Repeat(chars, 6)
                    .Select(s => s[random.Next(s.Length)]).ToArray());
                var existingUrlMap = db.UrlMaps.SingleOrDefault(u => u.ShortUrlKey == shortKey);
                if (existingUrlMap == null)
                {
                    return shortKey;
                }
            }
        }


    }
}