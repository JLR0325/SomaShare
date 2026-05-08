using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SomaShare.Data;
using SomaShare.Models;
using System.Linq;

namespace SomaShare.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Public home page (anyone can see)
        [AllowAnonymous]
        public IActionResult Public()
        {
            return View("PublicHome");
        }

        // Logged-in home page (only authenticated users can see)
        [Authorize]
        public IActionResult LoggedInHome()
        {
            // Fetch all textbook listings from the database
            var listings = _context.Textbooks.ToList();

            // Pass the listings to the view
            return View("LoggedInHome", listings);
        }

        // Optional: keep Index as a redirect to the right home
        [AllowAnonymous]
        public IActionResult Index()
        {
            if (User.Identity?.IsAuthenticated ?? false)
            {
                return RedirectToAction("LoggedInHome");
            }
            return RedirectToAction("Public");
        }
    }
}
