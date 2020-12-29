using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Volo.Abp.Account.Localization;
using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;
using Volo.Abp.Identity;
using IdentityUser = Volo.Abp.Identity.IdentityUser;

namespace Volo.Abp.Account.Web.Pages.Account
{
    public abstract class AccountPageModel : AbpPageModel
    {
        public IAccountAppService AccountAppService { get; set; }
        public SignInManager<IdentityUser> SignInManager { get; set; }
        public IdentityUserManager UserManager { get; set; }
        public IdentitySecurityLogManager IdentitySecurityLogManager { get; set; }
        public IOptions<IdentityOptions> IdentityOptions { get; set; }

        protected AccountPageModel()
        {
            LocalizationResourceType = typeof(AccountResource);
            ObjectMapperContext = typeof(AbpAccountWebModule);
        }

        protected virtual void CheckIdentityErrors(IdentityResult identityResult)
        {
            if (!identityResult.Succeeded)
            {
                throw new UserFriendlyException("Operation failed: " + identityResult.Errors.Select(e => $"[{e.Code}] {e.Description}").JoinAsString(", "));
            }

            //identityResult.CheckErrors(LocalizationManager); //TODO: Get from old Abp
        }

        protected virtual void CheckCurrentTenant(Guid? tenantId)
        {
            if (CurrentTenant.Id != tenantId)
            {
                throw new ApplicationException($"Current tenant is different than given tenant. CurrentTenant.Id: {CurrentTenant.Id}, given tenantId: {tenantId}");
            }
        }
    }
}
