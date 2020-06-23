using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Volo.Abp.Account.Web.Pages.Account
{
    public class LoggedOutModel : AccountPageModel
    {
        [HiddenInput]
        [BindProperty(SupportsGet = true)]
        public string ClientName { get; set; }

        [HiddenInput]
        [BindProperty(SupportsGet = true)]
        public string SignOutIframeUrl { get; set; }

        [HiddenInput]
        [BindProperty(SupportsGet = true)]
        public string PostLogoutRedirectUri { get; set; }

        public virtual Task<IActionResult> OnGetAsync()
        {
            return Task.FromResult<IActionResult>(Page());
        }

        public virtual Task<IActionResult> OnPostAsync()
        {
            return Task.FromResult<IActionResult>(Page());
        }
    }
}
