using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Volo.Abp.Account.Web.Pages.Account
{
    public class SendSecurityCodeModel : AccountPageModel
    {
        public List<SelectListItem> Providers { get; set; }
        
        public async Task<IActionResult> OnGetAsync()
        {
            var user = await SignInManager.GetTwoFactorAuthenticationUserAsync();
            if (user == null)
            {
                return RedirectToPage("./Login");
            }

            return Page();

            //CheckCurrentTenant(await SignInManager.GetVerifiedTenantIdAsync());

            //Providers = (await UserManager.GetValidTwoFactorProvidersAsync(user))
            //    .Select(userProvider =>
            //        new SelectListItem
            //        {
            //            Text = userProvider,
            //            Value = userProvider
            //        }).ToList();

            //return View(
            //    new SendSecurityCodeViewModel
            //    {
            //        ReturnUrl = returnUrl,
            //        RememberMe = rememberMe
            //    }
            //);
        }

    }
}