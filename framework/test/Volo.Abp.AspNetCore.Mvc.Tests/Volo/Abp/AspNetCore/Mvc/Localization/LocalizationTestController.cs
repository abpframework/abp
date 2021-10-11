using System.Globalization;
using Microsoft.AspNetCore.Mvc;

namespace Volo.Abp.AspNetCore.Mvc.Localization
{
    [Route("api/LocalizationTestController")]
    public class LocalizationTestController : AbpController
    {
        [HttpGet]
        public string Culture()
        {
            return CultureInfo.CurrentCulture.Name + ":" + CultureInfo.CurrentUICulture.Name;
        }
    }
}
