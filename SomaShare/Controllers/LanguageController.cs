using Microsoft.AspNetCore.Mvc;
using SomaShare.Services;

namespace SomaShare.Controllers
{
    public class LanguageController : Controller
    {
        private readonly LanguageService _languageService;
        public LanguageController(LanguageService languageService) => _languageService = languageService;

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Toggle(string returnUrl)
        {
            _languageService.ToggleLanguage();
            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                return LocalRedirect(returnUrl);
            return RedirectToAction("Public", "Home");
        }
    }
}
