using System.ComponentModel.DataAnnotations;

namespace URLShortner.Models
{
    public class UrlMap
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string LongUrl { get; set; }

        [Required]
        public string ShortUrlKey { get; set; }
    }
}