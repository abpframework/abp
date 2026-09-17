using Microsoft.AspNetCore.Mvc;

namespace Volo.Abp.AspNetCore.Mvc.Security.Headers;

[Route("SecurityHeadersTest")]
public class SecurityHeadersTestController : AbpController
{
    [HttpGet("Get")]
    public ActionResult Get()
    {
        return Content("OK");
    }

    [HttpGet("ignored")]
    public ActionResult Get_Ignored()
    {
        return Content("OK");
    }
}
