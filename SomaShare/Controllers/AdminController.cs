using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SomaShare.Data;

namespace SomaShare.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;
        public AdminController(ApplicationDbContext context) => _context = context;

        public IActionResult Dashboard()
        {
            ViewBag.UserCount = _context.Users.Count();
            ViewBag.ListingCount = _context.Textbooks.Count();
            ViewBag.TransactionCount = _context.Transactions.Count();
            return View();
        }
    }
}