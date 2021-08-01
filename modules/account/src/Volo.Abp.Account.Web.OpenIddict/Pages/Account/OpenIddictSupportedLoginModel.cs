using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.Account.Web.Pages.Account
{
    [ExposeServices(typeof(LoginModel))]
    public class OpenIddictSupportedLoginModel : LoginModel
    {
        public OpenIddictSupportedLoginModel(
            IAuthenticationSchemeProvider schemeProvider,
            IOptions<AbpAccountOptions> accountOptions,
            IOptions<IdentityOptions> identityOptions)
            : base(
                schemeProvider,
                accountOptions,
                identityOptions)
        {
        }

        public override async Task<IActionResult> OnPostExternalLogin(string provider)
        {
            if (AccountOptions.WindowsAuthenticationSchemeName == provider)
            {
                return await ProcessWindowsLoginAsync();
            }

            return await base.OnPostExternalLogin(provider);
        }

        protected virtual async Task<IActionResult> ProcessWindowsLoginAsync()
        {
            var result = await HttpContext.AuthenticateAsync(AccountOptions.WindowsAuthenticationSchemeName);
            if (result.Succeeded)
            {
                var props = new AuthenticationProperties()
                {
                    RedirectUri = Url.Page("./Login", pageHandler: "ExternalLoginCallback", values: new { ReturnUrl, ReturnUrlHash }),
                    Items =
                    {
                        {
                            "LoginProvider", AccountOptions.WindowsAuthenticationSchemeName
                        },
                    }
                };

                var id = new ClaimsIdentity(AccountOptions.WindowsAuthenticationSchemeName);
                id.AddClaim(new Claim(ClaimTypes.NameIdentifier, result.Principal.FindFirstValue(ClaimTypes.PrimarySid)));
                id.AddClaim(new Claim(ClaimTypes.Name, result.Principal.FindFirstValue(ClaimTypes.Name)));

                await HttpContext.SignInAsync(IdentityConstants.ExternalScheme, new ClaimsPrincipal(id), props);

                return Redirect(props.RedirectUri);
            }

            return Challenge(AccountOptions.WindowsAuthenticationSchemeName);
        }
    }
}
