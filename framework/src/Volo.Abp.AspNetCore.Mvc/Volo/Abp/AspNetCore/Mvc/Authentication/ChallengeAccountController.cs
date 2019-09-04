using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace Volo.Abp.AspNetCore.Mvc.Authentication
{
    public abstract class ChallengeAccountController : AbpController
    {
        protected string[] ChallengeAuthenticationSchemas { get; }

        protected ChallengeAccountController(string[] challengeAuthenticationSchemas = null)
        {
            ChallengeAuthenticationSchemas = challengeAuthenticationSchemas ?? new[]{ "oidc" };
        }

        [HttpGet]
        public ActionResult Login(string returnUrl = "", string returnUrlHash = "")
        {
            if (CurrentUser.IsAuthenticated)
            {
                return RedirectSafely(returnUrl, returnUrlHash);
            }
            else
            {
                return Challenge(
                    new AuthenticationProperties
                    {
                        Parameters =
                        {
                            {"returnUrl", returnUrl},
                            {"returnUrlHash", returnUrlHash}
                        }
                    },
                    ChallengeAuthenticationSchemas
                );
            }
        }

        [HttpGet]
        public async Task<ActionResult> Logout(string returnUrl = "", string returnUrlHash = "")
        {
            await HttpContext.SignOutAsync();

            return RedirectSafely(returnUrl, returnUrlHash);
        }

        protected RedirectResult RedirectSafely(string returnUrl, string returnUrlHash = null)
        {
            return Redirect(GetRedirectUrl(returnUrl, returnUrlHash));
        }

        private string GetRedirectUrl(string returnUrl, string returnUrlHash = null)
        {
            returnUrl = NormalizeReturnUrl(returnUrl);

            if (!returnUrlHash.IsNullOrWhiteSpace())
            {
                returnUrl = returnUrl + returnUrlHash;
            }

            return returnUrl;
        }

        private string NormalizeReturnUrl(string returnUrl)
        {
            if (returnUrl.IsNullOrEmpty())
            {
                return GetAppHomeUrl();
            }

            if (Url.IsLocalUrl(returnUrl))
            {
                return returnUrl;
            }

            return GetAppHomeUrl();
        }

        protected virtual string GetAppHomeUrl()
        {
            return "/";
        }
    }
}