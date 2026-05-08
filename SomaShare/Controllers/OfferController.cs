using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SomaShare.Models;
using SomaShare.Services;
using System.Security.Claims;

namespace SomaShare.Controllers
{
    [Authorize]
    public class OfferController : Controller
    {
        private readonly OfferService _offerService;
        private readonly TextbookService _textbookService;

        public OfferController(OfferService offerService, TextbookService textbookService)
        {
            _offerService = offerService;
            _textbookService = textbookService;
        }

        public async Task<IActionResult> Create(int textbookId)
        {
            var book = await _textbookService.GetByIdAsync(textbookId);
            if (book == null || book.UserId == User.FindFirstValue(ClaimTypes.NameIdentifier))
                return RedirectToAction("Details", "Textbook", new { id = textbookId });
            ViewBag.BookTitle = book.Title;
            return View(new Offer { TextbookId = textbookId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Offer offer)
        {
            if (!ModelState.IsValid) return View(offer);
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            var result = await _offerService.CreateOfferAsync(offer, userId);
            if (!result) return View("Error");
            return RedirectToAction("MyOffers");
        }

        public async Task<IActionResult> MyOffers()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            var offers = await _offerService.GetOffersByBuyerAsync(userId);
            return View(offers);
        }

        public async Task<IActionResult> SellerOffers()
        {
            var sellerId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            var offers = await _offerService.GetOffersForSellerAsync(sellerId);
            return View(offers);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Accept(int offerId)
        {
            var sellerId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            await _offerService.AcceptOfferAsync(offerId, sellerId);
            return RedirectToAction("SellerOffers");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Reject(int offerId)
        {
            var sellerId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            await _offerService.RejectOfferAsync(offerId, sellerId);
            return RedirectToAction("SellerOffers");
        }
    }
}