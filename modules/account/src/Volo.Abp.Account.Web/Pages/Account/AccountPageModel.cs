using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Volo.Abp.Account.Localization;
using Volo.Abp.AspNetCore.ExceptionHandling;
using Volo.Abp.AspNetCore.MultiTenancy;
using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;
using Volo.Abp.ExceptionHandling;
using Volo.Abp.Identity;
using Volo.Abp.MultiTenancy;
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
        public IExceptionToErrorInfoConverter ExceptionToErrorInfoConverter { get; set; }

        public ITenantResolveResultAccessor TenantResolveResultAccessor { get; set; }

        public IOptions<AbpAspNetCoreMultiTenancyOptions> AspNetCoreMultiTenancyOptions { get; set; }

        public IOptions<AbpMultiTenancyOptions> MultiTenancyOptions { get; set; }

        protected AccountPageModel()
        {
            LocalizationResourceType = typeof(AccountResource);
            ObjectMapperContext = typeof(AbpAccountWebModule);
        }

        protected virtual bool SwitchTenant(Guid? tenantId)
        {
            if (MultiTenancyOptions.Value.IsEnabled &&
                TenantResolveResultAccessor.Result?.AppliedResolvers?.Contains(CookieTenantResolveContributor.ContributorName) == true)
            {
                if (CurrentTenant.Id != tenantId)
                {
                    if (tenantId != null)
                    {
                        Response.Cookies.Append(
                            AspNetCoreMultiTenancyOptions.Value.TenantKey,
                            tenantId.ToString(),
                            new CookieOptions
                            {
                                Path = "/",
                                HttpOnly = false,
                                Expires = DateTimeOffset.Now.AddYears(10)
                            }
                        );
                    }
                    else
                    {
                        Response.Cookies.Delete(AspNetCoreMultiTenancyOptions.Value.TenantKey);
                    }

                    return true;
                }
            }

            return false;
        }

        protected virtual void CheckIdentityErrors(IdentityResult identityResult)
        {
            if (!identityResult.Succeeded)
            {
                throw new UserFriendlyException("Operation failed: " + identityResult.Errors.Select(e => $"[{e.Code}] {e.Description}").JoinAsString(", "));
            }

            //identityResult.CheckErrors(LocalizationManager); //TODO: Get from old Abp
        }

        protected virtual string GetLocalizeExceptionMessage(Exception exception)
        {
            if (exception is ILocalizeErrorMessage || exception is IHasErrorCode)
            {
                return ExceptionToErrorInfoConverter.Convert(exception, false).Message;
            }

            return exception.Message;
        }
    }
}
