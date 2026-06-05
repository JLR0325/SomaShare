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
            // Read optional pageSize from query string if provided (e.g. ?pageSize=20)
            var pageSize = HttpContext.Request.Query.ContainsKey("pageSize")
                ? (int?)System.Convert.ToInt32(HttpContext.Request.Query["pageSize"].ToString())
                : null;
            var myListings = await _textbookService.GetByUserAsync(userId!);
            var myOffers = await _offerService.GetOffersByBuyerAsync(userId!);
            var myTransactions = await _transactionService.GetUserTransactionsAsync(userId!);
            var model = new
            {
                MyListings = myListings,
                MyOffers = myOffers,
                MyTransactions = myTransactions,
                PageSize = pageSize
            };
            return View(model);
        }
    }
}