using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.Account.Web.Areas.Account.Models.Login;
using Volo.Abp.Identity;
using Volo.Abp.Ui;

namespace Volo.Abp.Account.Web.Areas.Account.Controllers
{
    [Area("Account")]
    public class LoginController : AccountControllerBase
    {
        private readonly SignInManager<IdentityUser> _signInManager;

        public LoginController(SignInManager<IdentityUser> signInManager)
        {
            _signInManager = signInManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(LoginModel loginModel, string returnUrl = "", string returnUrlHash = "")
        {
            if (!ModelState.IsValid)
            {
                throw new NotImplementedException();
            }

            var result = await _signInManager.PasswordSignInAsync(
                loginModel.UserNameOrEmailAddress,
                loginModel.Password,
                loginModel.RememberMe,
                true
            );

            if (!result.Succeeded)
            {
                throw new UserFriendlyException("Login failed!"); //TODO: Handle other cases, do not throw exception
            }

            return RedirectSafely(returnUrl, returnUrlHash);
        }
    }
}
