using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SomaShare.Models;
using SomaShare.Services;
using System.Security.Claims;

namespace SomaShare.Controllers
{
    [Authorize]
    public class ReviewController : Controller
    {
        private readonly ReviewService _reviewService;
        private readonly UserManager<ApplicationUser> _userManager;

        public ReviewController(ReviewService reviewService, UserManager<ApplicationUser> userManager)
        {
            _reviewService = reviewService;
            _userManager = userManager;
        }

        public async Task<IActionResult> Create(string reviewedUserId)
        {
            var user = await _userManager.FindByIdAsync(reviewedUserId);
            if (user == null) return NotFound();
            ViewBag.ReviewedUserName = user.FullName;
            ViewBag.ReviewedUserId = reviewedUserId;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int rating, string? comment, string reviewedUserId)
        {
            var reviewerId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            if (reviewerId == reviewedUserId) return BadRequest("Cannot review yourself.");
            await _reviewService.CreateReviewAsync(rating, comment, reviewerId, reviewedUserId);
            return RedirectToAction("MyReviews");
        }

        public async Task<IActionResult> MyReviews()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            var reviews = await _reviewService.GetReviewsForUserAsync(userId);
            return View(reviews);
        }
    }
}