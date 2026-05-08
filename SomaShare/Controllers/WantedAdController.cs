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
        public async Task<IActionResult> Create(WantedAd ad)
        {
            if (!ModelState.IsValid) return View(ad);
            ad.UserId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            ad.PostedDate = DateTime.UtcNow;
            await _service.CreateAsync(ad);
            return RedirectToAction(nameof(Index));
        }

        // Add Edit/Delete actions if needed
    }
}
