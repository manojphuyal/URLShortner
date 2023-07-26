using System.Data.Entity;

namespace URLShortner.Models
{
    public class UrlShortenerDbContext : DbContext
    {
        public DbSet<UrlMap> UrlMaps { get; set; }
        public UrlShortenerDbContext() : base("DefaultConnection")
        {
        }
    }
}