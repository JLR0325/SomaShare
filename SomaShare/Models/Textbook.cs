using System.ComponentModel.DataAnnotations;

namespace SomaShare.Models
{
    public class Textbook
    {
        public int Id { get; set; }
        [Required, StringLength(150)]
        public string Title { get; set; } = string.Empty;
        [Required, StringLength(100)]
        public string Author { get; set; } = string.Empty;
        [StringLength(20)]
        public string ISBN { get; set; } = string.Empty;
        [StringLength(30)]
        public string Edition { get; set; } = string.Empty;
        [Required, StringLength(50)]
        public string Condition { get; set; } = string.Empty; // New, Like New, Good, Fair
        [Range(0, 10000)]
        public decimal Price { get; set; }
        [StringLength(80)]
        public string Campus { get; set; } = string.Empty;
        public string? ImageUrl { get; set; } = string.Empty;
        public DateTime ListedDate { get; set; } = DateTime.UtcNow;
        public bool IsSold { get; set; } = false;

        // FK to seller (User)
        public string UserId { get; set; } = string.Empty;
        public ApplicationUser User { get; set; } = null!;

        public ICollection<Offer> Offers { get; set; } = new List<Offer>();
    }
}