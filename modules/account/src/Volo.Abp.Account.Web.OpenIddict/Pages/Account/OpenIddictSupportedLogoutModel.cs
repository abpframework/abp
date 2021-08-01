using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OpenIddict.Server.AspNetCore;
using System.Security.Claims;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Identity;

namespace Volo.Abp.Account.Web.Pages.Account
{
    [ExposeServices(typeof(LogoutModel))]
    public class OpenIddictSupportedLogoutModel : LogoutModel
    {
        public async override Task<IActionResult> OnGetAsync()
        {
            await SignInManager.SignOutAsync();

            var request = HttpContext.GetOpenIddictServerRequest();

            if (request != null)
            {
                await SaveSecurityLogAsync(request?.ClientId);

                await SignInManager.SignOutAsync();
                await HttpContext.SignOutAsync(OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);

                HttpContext.User = new ClaimsPrincipal(new ClaimsIdentity());

                var vm = new LoggedOutModel()
                {
                    PostLogoutRedirectUri = request?.PostLogoutRedirectUri,
                    ClientName = request?.Display,
                    SignOutIframeUrl = null
                };

                Logger.LogInformation($"Redirecting to LoggedOut Page...");

                return RedirectToPage("./LoggedOut", vm);
            }

            await SaveSecurityLogAsync();

            if (ReturnUrl != null)
            {
                return LocalRedirect(ReturnUrl);
            }

            Logger.LogInformation(
                $"OpenIddictSupportedLogoutModel couldn't find postLogoutUri... Redirecting to:/Account/Login..");

            return SignOut(
                authenticationSchemes: OpenIddictServerAspNetCoreDefaults.AuthenticationScheme,
                properties: new AuthenticationProperties
                {
                    RedirectUri = "/Account/Login"
                });
        }

        protected virtual async Task SaveSecurityLogAsync(string clientId = null)
        {
            if (CurrentUser.IsAuthenticated)
            {
                await IdentitySecurityLogManager.SaveAsync(new IdentitySecurityLogContext()
                {
                    Identity = IdentitySecurityLogIdentityConsts.Identity,
                    Action = IdentitySecurityLogActionConsts.Logout,
                    ClientId = clientId
                });
            }
        }
    }
}
