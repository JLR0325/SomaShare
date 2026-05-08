using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SomaShare.Models;
using SomaShare.Services;
using System.Security.Claims;

namespace SomaShare.Controllers
{
    [Authorize]
    public class TextbookController : Controller
    {
        private readonly TextbookService _textbookService;
        public TextbookController(TextbookService textbookService) => _textbookService = textbookService;

        [AllowAnonymous]
        public async Task<IActionResult> Index(string searchString, string condition, string campus,
            decimal? minPrice, decimal? maxPrice, string sortOrder, int page = 1)
        {
            var books = await _textbookService.GetAllAsync(searchString, condition, campus,
                minPrice, maxPrice, sortOrder, page);
            ViewBag.SearchString = searchString;
            ViewBag.Condition = condition;
            ViewBag.Campus = campus;
            ViewBag.MinPrice = minPrice;
            ViewBag.MaxPrice = maxPrice;
            ViewBag.SortOrder = sortOrder;
            ViewBag.Page = page;
            return View(books);
        }

        [AllowAnonymous]
        public async Task<IActionResult> Details(int id)
        {
            var book = await _textbookService.GetByIdAsync(id);
            if (book == null) return NotFound();
            return View(book);
        }

        public IActionResult Create() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Textbook textbook)
        {
            if (!ModelState.IsValid) return View(textbook);
            textbook.UserId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            await _textbookService.CreateAsync(textbook);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var book = await _textbookService.GetByIdAsync(id);
            if (book == null || book.UserId != User.FindFirstValue(ClaimTypes.NameIdentifier))
                return Forbid();
            return View(book);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Textbook textbook)
        {
            if (!ModelState.IsValid) return View(textbook);
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            var existing = await _textbookService.GetByIdAsync(textbook.Id);
            if (existing == null || existing.UserId != userId) return Forbid();
            await _textbookService.UpdateAsync(textbook);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var book = await _textbookService.GetByIdAsync(id);
            if (book == null || book.UserId != User.FindFirstValue(ClaimTypes.NameIdentifier))
                return Forbid();
            return View(book);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            await _textbookService.DeleteAsync(id, userId);
            return RedirectToAction(nameof(Index));
        }
    }
}