using SomaShare.Models;

namespace SomaShare.Controllers
{
    public class UserProfileViewModel
    {
        public ApplicationUser User { get; set; } = null!;
        public List<Review> Reviews { get; set; } = new List<Review>();
        public double AverageRating { get; set; }
        public int ReviewCount { get; set; }
        public List<Textbook> TextbookListings { get; set; } = new List<Textbook>();
        public List<WantedAd> WantedAdListings { get; set; } = new List<WantedAd>();
    }
}
