using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Mime;

namespace IntexFagElGamous.Views.Shared
{
    public class _CookieConsentPartialModel : PageModel
    {
        public IActionResult OnGet()
        {
            var consentFeature = HttpContext.Features.Get<ITrackingConsentFeature>();
            if (consentFeature?.CanTrack ?? false)
            {
                // Cookies are allowed, no need to show the banner
                return new EmptyResult();
            }
            else
            {
                // Cookies are not allowed, show the banner
                var cookieString = consentFeature?.CreateConsentCookie();
                ViewData["CookieString"] = cookieString;
                return Page();
            }
        }
    }
}
