using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SomaShare.Data;
using SomaShare.Models;

namespace SomaShare.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public DashboardController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Dashboard/Index - User's dashboard
        public async Task<IActionResult> Index()
        {
            try
            {
                var currentUserId = _userManager.GetUserId(User);
                if (string.IsNullOrEmpty(currentUserId))
                    return RedirectToAction("Login", "Account");

                var user = await _userManager.FindByIdAsync(currentUserId);

                // Get user's listings
                var myListings = _context.Textbooks
                    .Where(t => t.UserId == currentUserId && !t.IsSold)
                    .ToList();

                // Get user's offers received
                var offersReceived = _context.Offers
                    .Where(o => o.TextbookUserId == currentUserId)
                    .ToList();

                // Get user's transactions
                var transactions = _context.Transactions
                    .Where(t => t.BuyerId == currentUserId || t.SellerId == currentUserId)
                    .ToList();

                // Get user's reviews
                var reviews = _context.Reviews
                    .Where(r => r.ReviewedUserId == currentUserId)
                    .ToList();

                ViewBag.MyListingsCount = myListings.Count();
                ViewBag.OffersCount = offersReceived.Count();
                ViewBag.TransactionsCount = transactions.Count();
                ViewBag.ReviewsCount = reviews.Count();

                return View();
            }
            catch (Exception ex)
            {
                return View("Error", new ErrorViewModel { RequestId = ex.Message });
            }
        }

        // GET: Dashboard/MyOffers - User's created offers
        public async Task<IActionResult> MyOffers()
        {
            try
            {
                var currentUserId = _userManager.GetUserId(User);
                if (string.IsNullOrEmpty(currentUserId))
                    return RedirectToAction("Login", "Account");

                var offers = _context.Offers
                    .Where(o => o.BuyerId == currentUserId)
                    .ToList();

                return View("~/Views/Offer/MyOffers.cshtml", offers);
            }
            catch (Exception ex)
            {
                return View("Error", new ErrorViewModel { RequestId = ex.Message });
            }
        }

        // GET: Dashboard/SellerOffers - Offers received on user's items
        public async Task<IActionResult> SellerOffers()
        {
            try
            {
                var currentUserId = _userManager.GetUserId(User);
                if (string.IsNullOrEmpty(currentUserId))
                    return RedirectToAction("Login", "Account");

                var offers = _context.Offers
                    .Where(o => o.TextbookUserId == currentUserId)
                    .ToList();

                return View("~/Views/Offer/SellerOffers.cshtml", offers);
            }
            catch (Exception ex)
            {
                return View("Error", new ErrorViewModel { RequestId = ex.Message });
            }
        }

        // GET: Dashboard/MyTransactions - User's transactions
        public async Task<IActionResult> MyTransactions()
        {
            try
            {
                var currentUserId = _userManager.GetUserId(User);
                if (string.IsNullOrEmpty(currentUserId))
                    return RedirectToAction("Login", "Account");

                var transactions = _context.Transactions
                    .Where(t => t.BuyerId == currentUserId || t.SellerId == currentUserId)
                    .ToList();

                return View("~/Views/Transaction/MyTransactions.cshtml", transactions);
            }
            catch (Exception ex)
            {
                return View("Error", new ErrorViewModel { RequestId = ex.Message });
            }
        }

        // GET: Dashboard/MyReviews - Reviews about the user
        public async Task<IActionResult> MyReviews()
        {
            try
            {
                var currentUserId = _userManager.GetUserId(User);
                if (string.IsNullOrEmpty(currentUserId))
                    return RedirectToAction("Login", "Account");

                var reviews = _context.Reviews
                    .Where(r => r.ReviewedUserId == currentUserId)
                    .ToList();

                return View("~/Views/Review/MyReviews.cshtml", reviews);
            }
            catch (Exception ex)
            {
                return View("Error", new ErrorViewModel { RequestId = ex.Message });
            }
        }
    }
}
