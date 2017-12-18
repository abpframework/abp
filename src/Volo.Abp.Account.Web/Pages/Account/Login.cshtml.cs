using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.Identity;
using Volo.Abp.Ui;
using Volo.Abp.Uow;

namespace Volo.Abp.Account.Web.Pages.Account
{
    public class LoginModel : AccountModelBase
    {
        public string ReturnUrl { get; set; }  //TODO: Try to automatically bind from querystring!

        public string ReturnUrlHash { get; set; }  //TODO: Try to automatically bind from querystring!

        private readonly SignInManager<IdentityUser> _signInManager;

        public LoginModel(SignInManager<IdentityUser> signInManager)
        {
            _signInManager = signInManager;
        }

        public void OnGet(string returnUrl = "", string returnUrlHash = "")
        {
            ReturnUrl = returnUrl;
            ReturnUrlHash = returnUrl;
        }

        //TODO: Bind input to a property instead of getting as parameter..?
        [UnitOfWork] //TODO: Will be removed when we implement action filter
        public virtual async Task<IActionResult> OnPostAsync(PostInput input, string returnUrl = "", string returnUrlHash = "")
        {
            ReturnUrl = returnUrl;
            ReturnUrlHash = returnUrl;

            if (!ModelState.IsValid)
            {
                throw new NotImplementedException();
            }

            var result = await _signInManager.PasswordSignInAsync(
                input.UserNameOrEmailAddress,
                input.Password,
                input.RememberMe,
                true
            );

            if (!result.Succeeded)
            {
                throw new UserFriendlyException("Login failed!"); //TODO: Handle other cases, do not throw exception
            }

            //TODO: Use LocalRedirect and Url.GetLocalUrl methods instead of a custom one!
            return RedirectSafely(returnUrl, returnUrlHash);
        }

        public class PostInput
        {
            [Required]
            [StringLength(255)]
            public string UserNameOrEmailAddress { get; set; }

            [Required]
            [StringLength(32)]
            public string Password { get; set; }

            public bool RememberMe { get; set; }
        }
    }
}
