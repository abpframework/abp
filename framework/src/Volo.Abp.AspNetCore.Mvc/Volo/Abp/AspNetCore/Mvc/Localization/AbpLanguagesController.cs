using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;

namespace Volo.Abp.AspNetCore.Mvc.Localization
{
    [Area("Abp")]
    [Route("Abp/Languages/[action]")]
    public class AbpLanguagesController : AbpController
    {
        [HttpGet]
        public IActionResult Switch(string culture, string uiCulture = "") //TODO: Implement return URL
        {
            //TODO: Check allowed languages and so on...

            Response.Cookies.Append(CookieRequestCultureProvider.DefaultCookieName, $"c={culture}|uic={uiCulture ?? culture}");
            return Redirect("/");
        }
    }
}
