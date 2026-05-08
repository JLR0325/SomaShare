using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SomaShare.Models;
using SomaShare.Services;
using System.Security.Claims;

namespace SomaShare.Controllers
{
    [Authorize]
    public class WantedAdController : Controller
    {
        private readonly WantedAdService _service;
        public WantedAdController(WantedAdService service) => _service = service;

        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            var ads = await _service.GetAllAsync();
            return View(ads);
        }

        [AllowAnonymous]
        public async Task<IActionResult> Details(int id)
        {
            var ad = await _service.GetByIdAsync(id);
            if (ad == null) return NotFound();
            return View(ad);
        }

        public IActionResult Create() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Title,Description,OfferAmount")] WantedAd ad)
        {
            if (!ModelState.IsValid) return View(ad);
            ad.UserId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            ad.PostedDate = DateTime.UtcNow;

            var success = await _service.CreateAsync(ad);

            if (success)
            {
                // Redirect to Details page of the newly created ad
                return RedirectToAction(nameof(Details), new { id = ad.Id });
            }

            // If save failed, stay on the form
            return View(ad);
        }

        // Edit/Delete actions
        public async Task<IActionResult> Edit(int id)
        {
            var ad = await _service.GetByIdAsync(id);
            if (ad == null) return NotFound();
            return View(ad);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(WantedAd ad)
        {
            if (!ModelState.IsValid) return View(ad);
            await _service.UpdateAsync(ad);
            return RedirectToAction(nameof(Details), new { id = ad.Id });
        }

        public async Task<IActionResult> Delete(int id)
        {
            var ad = await _service.GetByIdAsync(id);
            if (ad == null) return NotFound();
            return View(ad);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            await _service.DeleteAsync(id, userId);
            return RedirectToAction(nameof(Index));
        }


    }
}
