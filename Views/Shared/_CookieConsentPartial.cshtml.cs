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
            return Page();
        }
    }
}
