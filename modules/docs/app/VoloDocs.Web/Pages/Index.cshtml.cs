using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;
using Volo.Docs;

namespace VoloDocs.Web.Pages
{
    public class IndexModel : PageModel
    {
        private readonly DocsUrlOptions _urlOptions;

        public IndexModel(IOptions<DocsUrlOptions> urlOptions)
        {
            _urlOptions = urlOptions.Value;
        }

        public IActionResult OnGet()
        {
            //TODO: Create HomeController & Index instead of Page. Otherwise, we have an empty Index.cshtml file.
            if (!_urlOptions.RoutePrefix.IsNullOrWhiteSpace())
            {
                return Redirect("." + _urlOptions.GetFormattedRoutePrefix());
            }

            return Page();
        }
    }
}