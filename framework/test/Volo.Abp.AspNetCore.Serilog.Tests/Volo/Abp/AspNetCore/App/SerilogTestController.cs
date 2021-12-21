using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Tracing;

namespace Volo.Abp.AspNetCore.App;

public class SerilogTestController : AbpController
{
    private readonly ICorrelationIdProvider _correlationIdProvider;

    public SerilogTestController(ICorrelationIdProvider correlationIdProvider)
    {
        _correlationIdProvider = correlationIdProvider;
    }

    public ActionResult Index()
    {
        return Content("Index-Result");
    }

    public ActionResult CorrelationId()
    {
        return Content(_correlationIdProvider.Get());
    }
}
