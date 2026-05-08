using System.ComponentModel.DataAnnotations;

namespace SomaShare.Models
{
    public class Review
    {
        public int Id { get; set; }
        [Range(1, 5)]
        public int Rating { get; set; }
        public string? Comment { get; set; }
        public DateTime ReviewDate { get; set; } = DateTime.UtcNow;

        public string ReviewerId { get; set; } = string.Empty; // who left review
        public ApplicationUser Reviewer { get; set; } = null!;

        public string ReviewedUserId { get; set; } = string.Empty; // user being reviewed
        public ApplicationUser ReviewedUser { get; set; } = null!;
    }
}