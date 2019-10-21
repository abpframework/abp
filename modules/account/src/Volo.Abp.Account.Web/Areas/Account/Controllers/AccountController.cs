using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.Account.Web.Areas.Account.Controllers.Models;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Identity;
using Volo.Abp.Validation;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;
using UserLoginInfo = Volo.Abp.Account.Web.Areas.Account.Controllers.Models.UserLoginInfo;
using IdentityUser = Volo.Abp.Identity.IdentityUser;

namespace Volo.Abp.Account.Web.Areas.Account.Controllers
{
    [RemoteService]
    [Controller]
    [ControllerName("Login")]
    [Area("Account")]
    [Route("api/account")]
    public class AccountController : AbpController
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IdentityUserManager _userManager;

        public AccountController(SignInManager<IdentityUser> signInManager, IdentityUserManager userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }

        [HttpPost]
        [Route("login")]
        public virtual async Task<AbpLoginResult> Login(UserLoginInfo login)
        {
            ValidateLoginInfo(login);

            await ReplaceEmailToUsernameOfInputIfNeeds(login);

            return GetAbpLoginResult(await _signInManager.PasswordSignInAsync(
                login.UserNameOrEmailAddress,
                login.Password,
                login.RememberMe,
                true
            ));
        }

        [HttpPost]
        [Route("checkPassword")]
        public virtual async Task<AbpLoginResult> CheckPassword(UserLoginInfo login)
        {
            ValidateLoginInfo(login);
            var identityUser = await _userManager.FindByNameAsync(login.UserNameOrEmailAddress);

            if (identityUser == null)
            {
                return new AbpLoginResult(LoginResultType.InvalidUserNameOrPassword);
            }

            return GetAbpLoginResult(await _signInManager.CheckPasswordSignInAsync(identityUser, login.Password, true));
        }

        protected virtual async Task ReplaceEmailToUsernameOfInputIfNeeds(UserLoginInfo login)
        {
            if (!ValidationHandler.IsValidEmailAddress(login.UserNameOrEmailAddress))
            {
                return;
            }

            var userByUsername = await _userManager.FindByNameAsync(login.UserNameOrEmailAddress);
            if (userByUsername != null)
            {
                return;
            }

            var userByEmail = await _userManager.FindByEmailAsync(login.UserNameOrEmailAddress);
            if (userByEmail == null)
            {
                return;
            }

            login.UserNameOrEmailAddress = userByEmail.UserName;
        }

        private static AbpLoginResult GetAbpLoginResult(SignInResult result)
        {
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

            return new AbpLoginResult(LoginResultType.Success);
        }

        private void ValidateLoginInfo(UserLoginInfo login)
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
        }
    }
}
