using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;

namespace SomaShare.Controllers
{
    public class LanguageController : Controller
    {
        // GET: Language/SetLanguage
        [HttpGet]
        public IActionResult SetLanguage(string culture, string returnUrl)
        {
            if (!string.IsNullOrEmpty(culture))
            {
                Response.Cookies.Append(
                    CookieRequestCultureProvider.DefaultCookieName,
                    CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
                    new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) }
                );
            }

            // Return to the page that called this action, or go to home
            return LocalRedirect(string.IsNullOrEmpty(returnUrl) ? "/" : returnUrl);
        }
    }
}
