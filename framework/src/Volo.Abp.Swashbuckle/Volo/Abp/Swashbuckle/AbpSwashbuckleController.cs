using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.AntiForgery;
using Volo.Abp.Auditing;

namespace Volo.Abp.Swashbuckle;

[Area("Abp")]
[Route("Abp/Swashbuckle/[action]")]
[DisableAuditing]
[RemoteService(false)]
[ApiExplorerSettings(IgnoreApi = true)]
public class AbpSwashbuckleController : AbpController
{
    private readonly IAbpAntiForgeryManager _antiForgeryManager;

    public AbpSwashbuckleController(IAbpAntiForgeryManager antiForgeryManager)
    {
        _antiForgeryManager = antiForgeryManager;
    }

    [HttpGet]
    public void SetCsrfCookie()
    {
        _antiForgeryManager.SetCookie();
    }
}
