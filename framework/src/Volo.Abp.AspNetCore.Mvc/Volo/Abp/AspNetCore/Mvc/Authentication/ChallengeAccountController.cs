using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace Volo.Abp.AspNetCore.Mvc.Authentication
{
    public abstract class ChallengeAccountController : AbpController
    {
        protected string[] ChallengeAuthenticationSchemas { get; }
        protected string AuthenticationType { get; }

        protected ChallengeAccountController(string[] challengeAuthenticationSchemas = null)
        {
            ChallengeAuthenticationSchemas = challengeAuthenticationSchemas ?? new[] { "oidc" };
            AuthenticationType = "Identity.Application";
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

            if (HttpContext.User.Identity.AuthenticationType == AuthenticationType)
            {
                return RedirectSafely(returnUrl, returnUrlHash);
            }

            return new SignOutResult(ChallengeAuthenticationSchemas);
        }

        [HttpGet]
        public async Task<IActionResult> FrontChannelLogout(string sid)
        {
            if (User.Identity.IsAuthenticated)
            {
                var currentSid = User.FindFirst("sid").Value ?? string.Empty;
                if (string.Equals(currentSid, sid, StringComparison.Ordinal))
                {
                    await Logout();
                }
            }

            return NoContent();
        }
    }
}
