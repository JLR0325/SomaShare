using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SomaShare.Services;
using System.Security.Claims;

namespace SomaShare.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {
        private readonly TextbookService _textbookService;
        private readonly OfferService _offerService;
        private readonly TransactionService _transactionService;

        public DashboardController(TextbookService ts, OfferService os, TransactionService trs)
        {
            _textbookService = ts;
            _offerService = os;
            _transactionService = trs;
        }

        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var allBooks = await _textbookService.GetAllAsync(null, null, null, null, null, null);
            ViewBag.MyListings = allBooks.Where(b => b.UserId == userId).ToList();
            ViewBag.MyOffers = await _offerService.GetOffersByBuyerAsync(userId!);
            ViewBag.MyTransactions = await _transactionService.GetUserTransactionsAsync(userId!);
            return View();
        }
    }
}