using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Volo.Abp.Account.Web.Areas.Account.Controllers.Models;
using Volo.Abp.Account.Web.Localization;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Identity;
using UserLoginInfo = Volo.Abp.Account.Web.Areas.Account.Controllers.Models.UserLoginInfo;

namespace Volo.Abp.Account.Web.Areas.Account.Controllers
{
    [RemoteService]
    [Controller]
    [ControllerName("Login")]
    [Area("Account")]
    [Route("api/account/login")]
    public class LoginController : AbpController
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        public IStringLocalizer<AccountResource> L { get; set; }
        public string AspNetCoreIdentityCookieName = ".AspNetCore." + IdentityConstants.ApplicationScheme;

        public LoginController(SignInManager<IdentityUser> signInManager)
        {
            _signInManager = signInManager;
        }

        [HttpPost]
        [Route("")]
        public virtual async Task<AbpLoginResult> Login(UserLoginInfo login)
        {
            if (login == null)
            {
                throw new ArgumentException(nameof(login));
            }

            if (login.UserNameOrEmailAddress.IsNullOrEmpty())
            {
                throw new ArgumentNullException(nameof(login.UserNameOrEmailAddress));
            }

            if (login.Password.IsNullOrEmpty())
            {
                throw new ArgumentNullException(nameof(login.Password));
            }

            var result = await _signInManager.PasswordSignInAsync(
                login.UserNameOrEmailAddress,
                login.Password,
                login.RememberMe,
                true
            );

            if (result.IsLockedOut)
            {
                return new AbpLoginResult(LoginResultType.LockedOut);
            }

            if (result.RequiresTwoFactor)
            {
                return new AbpLoginResult(LoginResultType.RequiresTwoFactor);
            }

            if (result.IsNotAllowed)
            {
                return new AbpLoginResult(LoginResultType.NotAllowed);
            }

            if (!result.Succeeded)
            {
                return new AbpLoginResult(LoginResultType.InvalidUserNameOrPassword);
            }

            return new AbpLoginResult(LoginResultType.Success)
            {
                IdentityCookieToken = GetCookieValueFromResponse(AspNetCoreIdentityCookieName)
            };
        }

        private string GetCookieValueFromResponse(string cookieName)
        {
            foreach (var headers in Response.Headers.Values)
            {
                foreach (var header in headers)
                {
                    if (!header.StartsWith($"{cookieName}="))
                    {
                        continue;
                    }

                    var p1 = header.IndexOf('=');
                    var p2 = header.IndexOf(';');
                    return header.Substring(p1 + 1, p2 - p1 - 1);
                }
            }

            return null;
        }

    }
}
