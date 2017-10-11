using System;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Ui;

namespace Volo.Abp.Account.Web.Areas.Account.Controllers
{
    public abstract class AccountControllerBase : AbpController
    {
        protected RedirectResult RedirectSafely(string returnUrl, string returnUrlHash = null)
        {
            return Redirect(GetRedirectUrl(returnUrl, returnUrlHash));
        }

        protected void CheckIdentityErrors(IdentityResult identityResult)
        {
            if (!identityResult.Succeeded)
            {
                throw new UserFriendlyException("Operation failed!");
            }

            //identityResult.CheckErrors(LocalizationManager); //TODO: Get from old Abp
        }

        private string GetRedirectUrl(string returnUrl, string returnUrlHash = null)
        {
            returnUrl = NormalizeReturnUrl(returnUrl);

            if (!returnUrlHash.IsNullOrWhiteSpace())
            {
                returnUrl = returnUrl + returnUrlHash;
            }

            return returnUrl;
        }

        private string NormalizeReturnUrl(string returnUrl)
        {
            if (returnUrl.IsNullOrEmpty())
            {
                return GetAppHomeUrl();
            }

            if (Url.IsLocalUrl(returnUrl))
            {
                return returnUrl;
            }

            return GetAppHomeUrl();
        }

        private string GetAppHomeUrl()
        {
            return "/"; //TODO: ???
        }
    }
}
