using IdentityServer4.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.Account.Web.Pages.Account
{
    [ExposeServices(typeof(LogoutModel))]
    public class IdentityServerSupportedLogoutModel : LogoutModel
    {
        protected IIdentityServerInteractionService Interaction { get; }

        public IdentityServerSupportedLogoutModel(IIdentityServerInteractionService interaction)
        {
            Interaction = interaction;
        }

        public override async Task<IActionResult> OnGetAsync()
        {
            await SignInManager.SignOutAsync();

            var logoutId = Request.Query["logoutId"].ToString();

            if (!string.IsNullOrEmpty(logoutId))
            {
                var logoutContext = await Interaction.GetLogoutContextAsync(logoutId);

                var postLogoutUri = logoutContext.PostLogoutRedirectUri;

                if (!string.IsNullOrEmpty(postLogoutUri))
                {
                    return Redirect(postLogoutUri);
                }
            }

            if (ReturnUrl != null)
            {
                return LocalRedirect(ReturnUrl);
            }

            return RedirectToPage("/Account/Login");
        }
    }
}
