using Microsoft.AspNetCore.Mvc;
using Volo.Abp.Timing;

namespace Volo.Abp.AspNetCore.Mvc.Timing;

[Route("api/timing-test")]
public class UnitOfWorkTestController : AbpController
{
    private readonly ICurrentTimezoneProvider _currentTimezoneProvider;

    public UnitOfWorkTestController(ICurrentTimezoneProvider currentTimezoneProvider)
    {
        _currentTimezoneProvider = currentTimezoneProvider;
    }

    [HttpGet]
    public ActionResult GetAsync()
    {
        return Content(_currentTimezoneProvider.TimeZone ?? "null");
    }

    [HttpPost]
    public ActionResult PostAsync()
    {
        return Content(_currentTimezoneProvider.TimeZone ?? "null");
    }
}
