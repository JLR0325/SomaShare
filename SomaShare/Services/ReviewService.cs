using Microsoft.EntityFrameworkCore;
using SomaShare.Data;
using SomaShare.Models;

namespace SomaShare.Services
{
    public class ReviewService
    {
        private readonly ApplicationDbContext _context;
        public ReviewService(ApplicationDbContext context) => _context = context;

        public async Task<bool> CreateReviewAsync(int rating, string? comment, string reviewerId, string reviewedUserId)
        {
            var review = new Review
            {
                Rating = rating,
                Comment = comment,
                ReviewerId = reviewerId,
                ReviewedUserId = reviewedUserId
            };
            _context.Reviews.Add(review);
            var saved = await _context.SaveChangesAsync() > 0;
            if (saved)
            {
                // Recalculate average trust score
                var avg = await _context.Reviews
                    .Where(r => r.ReviewedUserId == reviewedUserId)
                    .AverageAsync(r => (double?)r.Rating) ?? 0;
                var user = await _context.Users.FindAsync(reviewedUserId);
                if (user != null)
                {
                    user.Rating = Math.Round(avg, 2);
                    await _context.SaveChangesAsync();
                }
            }
            return saved;
        }

        public async Task<List<Review>> GetReviewsForUserAsync(string userId) =>
            await _context.Reviews.Include(r => r.Reviewer)
                .Where(r => r.ReviewedUserId == userId)
                .OrderByDescending(r => r.ReviewDate)
                .ToListAsync();
    }
}