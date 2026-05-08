using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SomaShare.Services;
using System.Security.Claims;

namespace SomaShare.Controllers
{
    [Authorize]
    public class TransactionController : Controller
    {
        private readonly TransactionService _transactionService;
        public TransactionController(TransactionService transactionService) => _transactionService = transactionService;

        public async Task<IActionResult> MyTransactions()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            var txns = await _transactionService.GetUserTransactionsAsync(userId);
            return View(txns);
        }

        public async Task<IActionResult> Detail(int id)
        {
            var txn = await _transactionService.GetByIdAsync(id);
            if (txn == null) return NotFound();
            return View(txn);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> MarkCompleted(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            await _transactionService.MarkCompletedAsync(id, userId);
            return RedirectToAction("MyTransactions");
        }
    }
}