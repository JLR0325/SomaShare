using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SomaShare.Models;
using SomaShare.Services;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using SomaShare.Data;

namespace SomaShare.Controllers
{
    public class ProfileController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ReviewService _reviewService;
        private readonly ApplicationDbContext _context;

        public ProfileController(UserManager<ApplicationUser> userManager, 
                               ReviewService reviewService,
                               ApplicationDbContext context)
        {
            _userManager = userManager;
            _reviewService = reviewService;
            _context = context;
        }

        [AllowAnonymous]
        public async Task<IActionResult> View(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return NotFound();

            var reviews = await _reviewService.GetReviewsForUserAsync(id);
            var textbooks = await _context.Textbooks.Where(t => t.UserId == id).ToListAsync();
            var wantedAds = await _context.WantedAds.Where(w => w.UserId == id).ToListAsync();

            var viewModel = new UserProfileViewModel
            {
                User = user,
                Reviews = reviews,
                AverageRating = user.Rating,
                ReviewCount = reviews.Count,
                TextbookListings = textbooks,
                WantedAdListings = wantedAds
            };

            return View(viewModel);
        }

        [Authorize]
        public async Task<IActionResult> MyProfile()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return RedirectToAction(nameof(View), new { id = userId });
        }
    }
}
