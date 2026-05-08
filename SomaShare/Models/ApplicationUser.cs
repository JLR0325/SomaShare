using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace SomaShare.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Required, StringLength(100)]
        public string FullName { get; set; } = string.Empty;

        [StringLength(100)]
        public string Institution { get; set; } = string.Empty;

        [StringLength(80)]
        public string Course { get; set; } = string.Empty;

        public string Campus { get; set; } = string.Empty;

        public double Rating { get; set; } = 0;

        // New property for profile image
        [Url]
        public string? ProfileImageUrl { get; set; }

        // Helper method to always return a usable image URL
        public string GetProfileImageUrl()
        {
            return string.IsNullOrEmpty(ProfileImageUrl)
                ? "/images/default-profile.png"   // fallback placeholder
                : ProfileImageUrl;
        }

        // Navigation properties
        public ICollection<Textbook> Textbooks { get; set; } = new List<Textbook>();
        public ICollection<WantedAd> WantedAds { get; set; } = new List<WantedAd>();
        public ICollection<Offer> OffersMade { get; set; } = new List<Offer>();
        public ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
        public ICollection<Review> ReviewsWritten { get; set; } = new List<Review>();
        public ICollection<Review> ReviewsReceived { get; set; } = new List<Review>();
        public ICollection<ForumThread> ForumThreads { get; set; } = new List<ForumThread>();
        public ICollection<ForumPost> ForumPosts { get; set; } = new List<ForumPost>();
    }
}
