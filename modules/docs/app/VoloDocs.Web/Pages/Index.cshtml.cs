using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;
using Volo.Docs;

namespace VoloDocs.Web.Pages
{
    public class IndexModel : PageModel
    {
        private readonly DocsUiOptions _urlUiOptions;

        public IndexModel(IOptions<DocsUiOptions> urlOptions)
        {
            _urlUiOptions = urlOptions.Value;
        }

        public IActionResult OnGet()
        {
            //TODO: Create HomeController & Index instead of Page. Otherwise, we have an empty Index.cshtml file.
            if (!_urlUiOptions.RoutePrefix.IsNullOrWhiteSpace())
            {
                return Redirect("." + _urlUiOptions.RoutePrefix);
            }

            return Page();
        }
    }
}