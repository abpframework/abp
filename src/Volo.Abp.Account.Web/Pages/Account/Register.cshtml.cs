using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.Identity;
using Volo.Abp.Uow;

namespace Volo.Abp.Account.Web.Pages.Account
{
    public class RegisterModel : AccountModelBase
    {
        public string ReturnUrl { get; set; }  //TODO: Try to automatically bind from querystring!

        public string ReturnUrlHash { get; set; }  //TODO: Try to automatically bind from querystring!

        private readonly IdentityUserManager _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;

        public RegisterModel(IdentityUserManager userManager, SignInManager<IdentityUser> signInManager)
        {
            _userManager = userManager;
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
            if (!ModelState.IsValid)
            {
                throw new NotImplementedException();
            }

            var user = new IdentityUser(GuidGenerator.Create(), input.UserName, CurrentTenant.Id);

            var result = await _userManager.CreateAsync(user, input.Password);

            if (!result.Succeeded)
            {
                throw new NotImplementedException();
            }

            await _userManager.SetEmailAsync(user, input.EmailAddress);

            await _signInManager.SignInAsync(user, isPersistent: false);

            //TODO: Use LocalRedirect and Url.GetLocalUrl methods instead of a custom one!
            return RedirectSafely(returnUrl, returnUrlHash);
        }

        public class PostInput
        {
            [Required]
            [StringLength(32)]
            public string UserName { get; set; }

            [Required]
            [EmailAddress]
            [StringLength(255)]
            public string EmailAddress { get; set; }

            [Required]
            [StringLength(32)]
            public string Password { get; set; }
        }
    }
}
