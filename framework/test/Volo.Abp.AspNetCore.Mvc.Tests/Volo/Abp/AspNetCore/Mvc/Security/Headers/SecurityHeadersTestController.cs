using Microsoft.AspNetCore.Mvc;

namespace Volo.Abp.AspNetCore.Mvc.Security.Headers
{
    public class SecurityHeadersTestController : AbpController
    {
        public ActionResult Get()
        {
            return Content("OK");
        }
    }
}
