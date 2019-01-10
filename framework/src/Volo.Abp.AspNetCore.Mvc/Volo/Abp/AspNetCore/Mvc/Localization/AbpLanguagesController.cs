using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.Settings;

namespace Volo.Abp.AspNetCore.Mvc.Localization
{
    [Area("Abp")]
    [Route("Abp/Languages/[action]")]
    public class AbpLanguagesController : AbpController
    {
        public SettingManager SettingManager { get; set; }

        [HttpGet]
        public IActionResult Switch(string culture, string uiCulture = "", string returnUrl = "")
        {
            if (!GlobalizationHelper.IsValidCultureCode(culture))
            {
                throw new AbpException("Unknown language: " + culture + ". It must be a valid culture!");
            }

            if (!GlobalizationHelper.IsValidCultureCode(uiCulture))
            {
                throw new AbpException("Unknown language: " + uiCulture + ". It must be a valid culture!");
            }

            string cookieValue = CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture, uiCulture));

            Response.Cookies.Append(CookieRequestCultureProvider.DefaultCookieName, cookieValue, new CookieOptions
            {
                Expires = Clock.Now.AddYears(2),
                HttpOnly = true
            });

            if (!string.IsNullOrWhiteSpace(returnUrl))
            {
                return Redirect(returnUrl);
            }

            return Redirect("/");
        }
    }
}
