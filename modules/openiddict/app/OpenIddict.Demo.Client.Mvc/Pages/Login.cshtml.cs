using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace OpenIddict.Demo.Client.Pages;

public class Login : PageModel
{
    public IActionResult OnGet()
    {
        return Challenge(new AuthenticationProperties { RedirectUri = "/" }, "oidc");
    }
}
