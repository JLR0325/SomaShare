using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SomaShare.Data;
using SomaShare.Models;

namespace SomaShare.Controllers
{
    public class ProfileController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public ProfileController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Profile/View/userId
        public async Task<IActionResult> View(string userId)
        {
            if (string.IsNullOrEmpty(userId))
                return NotFound();

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return NotFound();

            var model = new UserProfileViewModel
            {
                Id = user.Id,
                FullName = user.FullName,
                Email = user.Email,
                Institution = user.Institution,
                Course = user.Course,
                Campus = user.Campus,
                Rating = user.Rating,
                ProfileImageUrl = user.ProfileImageUrl,
                TextbooksCount = _context.Textbooks.Count(t => t.UserId == userId && !t.IsSold),
                ReviewsCount = _context.Reviews.Count(r => r.ReviewedUserId == userId)
            };

            return View(model);
        }

        // GET: Profile/View - View current user's profile
        [Authorize]
        public async Task<IActionResult> MyProfile()
        {
            var userId = _userManager.GetUserId(User);
            return RedirectToAction("View", new { userId = userId });
        }

        // GET: Profile/Browse - Browse all users to chat/interact with
        public async Task<IActionResult> Browse(string search = "", string campus = "")
        {
            try
            {
                var currentUserId = _userManager.GetUserId(User);
                var usersQuery = _context.Users.AsQueryable();

                // Exclude current user
                if (!string.IsNullOrEmpty(currentUserId))
                    usersQuery = usersQuery.Where(u => u.Id != currentUserId);

                // Filter by search term (name, email, institution, course)
                if (!string.IsNullOrEmpty(search))
                {
                    usersQuery = usersQuery.Where(u =>
                        u.FullName.Contains(search) ||
                        u.Email.Contains(search) ||
                        u.Institution.Contains(search) ||
                        u.Course.Contains(search));
                }

                // Filter by campus
                if (!string.IsNullOrEmpty(campus))
                    usersQuery = usersQuery.Where(u => u.Campus == campus);

                var users = await usersQuery.OrderBy(u => u.FullName).ToListAsync();

                // Get distinct campuses for filter dropdown
                var campuses = await _context.Users
                    .Where(u => !string.IsNullOrEmpty(u.Campus))
                    .Select(u => u.Campus)
                    .Distinct()
                    .OrderBy(c => c)
                    .ToListAsync();

                ViewBag.CurrentSearch = search;
                ViewBag.CurrentCampus = campus;
                ViewBag.Campuses = campuses;
                ViewBag.CurrentUserId = currentUserId;

                return View(users);
            }
            catch (Exception ex)
            {
                return View("Error", new ErrorViewModel { RequestId = ex.Message });
            }
        }
    }
}
