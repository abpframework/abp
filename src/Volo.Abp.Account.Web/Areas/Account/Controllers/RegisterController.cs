using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.Account.Web.Areas.Account.Models.Register;
using Volo.Abp.Identity;

namespace Volo.Abp.Account.Web.Areas.Account.Controllers
{
    [Area("Account")]
    public class RegisterController : AccountControllerBase
    {
        private readonly IdentityUserManager _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;

        public RegisterController(IdentityUserManager userManager, SignInManager<IdentityUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        //TODO: [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(RegisterModel registerModel, string returnUrl = "", string returnUrlHash = "")
        {
            if (!ModelState.IsValid)
            {
                throw new NotImplementedException();
            }

            var user = new IdentityUser(GuidGenerator.Create(), registerModel.UserName);

            var result = await _userManager.CreateAsync(user, registerModel.Password);

            if (!result.Succeeded)
            {
                throw new NotImplementedException();
            }

            await _userManager.SetEmailAsync(user, registerModel.EmailAddress);

            await _signInManager.SignInAsync(user, isPersistent: false);

            return RedirectSafely(returnUrl, returnUrlHash);
        }
    }
}
