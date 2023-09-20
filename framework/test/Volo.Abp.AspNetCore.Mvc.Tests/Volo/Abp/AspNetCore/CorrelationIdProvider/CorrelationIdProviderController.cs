using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Tracing;

namespace Volo.Abp.AspNetCore.CorrelationIdProvider;

[Route("api/correlation")]
public class CorrelationIdProviderController : AbpController
{
    [HttpGet]
    [Route("get")]
    public string Get()
    {
        return this.HttpContext.RequestServices.GetRequiredService<ICorrelationIdProvider>().Get();
    }
}
