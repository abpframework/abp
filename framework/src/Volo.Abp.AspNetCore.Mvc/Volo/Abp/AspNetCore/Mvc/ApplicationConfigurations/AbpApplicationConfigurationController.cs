using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.AntiForgery;

namespace Volo.Abp.AspNetCore.Mvc.ApplicationConfigurations;

[Area("abp")]
[RemoteService(Name = "abp")]
[Route("api/abp/application-configuration")]
public class AbpApplicationConfigurationController : AbpControllerBase, IAbpApplicationConfigurationAppService
{
    private readonly IAbpApplicationConfigurationAppService _applicationConfigurationAppService;
    private readonly IAbpAntiForgeryManager _antiForgeryManager;

    public AbpApplicationConfigurationController(
        IAbpApplicationConfigurationAppService applicationConfigurationAppService,
        IAbpAntiForgeryManager antiForgeryManager)
    {
        _applicationConfigurationAppService = applicationConfigurationAppService;
        _antiForgeryManager = antiForgeryManager;
    }

    [HttpGet]
    public virtual async Task<ApplicationConfigurationDto> GetAsync()
    {
        _antiForgeryManager.SetCookie();
        return await _applicationConfigurationAppService.GetAsync();
    }
}
