using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.RequestSizeLimit;
using Volo.Abp.Content;

namespace Volo.Abp.AspNetCore.Mvc.RequestSizeLimit;

[Route("api/request-siz-limit")]
[AbpRequestSizeLimit(5)]
public class RequestSizeLimitController : AbpController
{
    [HttpPost]
    [Route("check")]
    public string RequestSizeLimitCheck(IRemoteStreamContent file)
    {
        return !ModelState.IsValid ? ModelState.ToString() : "ok";
    }

    [HttpPost]
    [DisableRequestSizeLimit]
    [Route("disable")]
    public string DisableRequestSizeLimit(IRemoteStreamContent file)
    {
        return !ModelState.IsValid ? ModelState.ToString() : "ok";
    }
}
