using System.ComponentModel.DataAnnotations;

namespace SomaShare.Models
{
    public class WantedAd
    {
        public int Id { get; set; }
        [Required, StringLength(150)]
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime PostedDate { get; set; } = DateTime.UtcNow;

        [Required]
        public decimal OfferAmount { get; set; }

        public string UserId { get; set; } = string.Empty;
        public ApplicationUser? User { get; set; }
    }
}