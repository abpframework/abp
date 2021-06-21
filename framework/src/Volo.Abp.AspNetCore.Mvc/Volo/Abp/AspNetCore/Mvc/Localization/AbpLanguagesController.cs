using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using System;
using Microsoft.AspNetCore.RequestLocalization;
using Volo.Abp.Localization;

namespace Volo.Abp.AspNetCore.Mvc.Localization
{
    [Area("Abp")]
    [Route("Abp/Languages/[action]")]
    [RemoteService(false)]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class AbpLanguagesController : AbpController
    {
        [HttpGet]
        public IActionResult Switch(string culture, string uiCulture = "", string returnUrl = "")
        {
            if (!CultureHelper.IsValidCultureCode(culture))
            {
                throw new AbpException("Unknown language: " + culture + ". It must be a valid culture!");
            }

            AbpRequestCultureCookieHelper.SetCultureCookie(
                HttpContext,
                new RequestCulture(culture, uiCulture)
            );

            if (!string.IsNullOrWhiteSpace(returnUrl))
            {
                return Redirect(GetRedirectUrl(returnUrl));
            }

            return Redirect("~/");
        }

        private string GetRedirectUrl(string returnUrl)
        {
            if (returnUrl.IsNullOrEmpty())
            {
                return "~/";
            }

            if (Url.IsLocalUrl(returnUrl))
            {
                return returnUrl;
            }

            return "~/";
        }
    }
}
