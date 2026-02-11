using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NUglify.Helpers;

namespace Volo.Abp.Account.Web.Pages.Account;

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

    public virtual async Task<IActionResult> OnGetAsync()
    {
        await NormalizeUrlAsync();
        return Page();
    }

    public virtual async Task<IActionResult> OnPostAsync()
    {
        await NormalizeUrlAsync();
        return Page();
    }
    
    protected virtual async Task NormalizeUrlAsync()
    {
        if (!PostLogoutRedirectUri.IsNullOrWhiteSpace())
        {
            PostLogoutRedirectUri = Url.Content(await GetRedirectUrlAsync(PostLogoutRedirectUri));
        }
        
        if(!SignOutIframeUrl.IsNullOrWhiteSpace())
        {
            SignOutIframeUrl = Url.Content(await GetRedirectUrlAsync(SignOutIframeUrl));
        }
    }
}
