using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.AntiForgery;

namespace Volo.Abp.AspNetCore.Mvc.ApplicationConfigurations;

[Area("abp")]
[RemoteService(Name = "abp")]
[Route("api/abp/application-configuration")]
public class AbpApplicationConfigurationController : AbpControllerBase, IAbpApplicationConfigurationAppService
{
    protected readonly IAbpApplicationConfigurationAppService ApplicationConfigurationAppService;
    protected readonly IAbpAntiForgeryManager AntiForgeryManager;

    public AbpApplicationConfigurationController(
        IAbpApplicationConfigurationAppService applicationConfigurationAppService,
        IAbpAntiForgeryManager antiForgeryManager)
    {
        ApplicationConfigurationAppService = applicationConfigurationAppService;
        AntiForgeryManager = antiForgeryManager;
    }

    [HttpGet]
    public virtual async Task<ApplicationConfigurationDto> GetAsync(
        ApplicationConfigurationRequestOptions options)
    {
        AntiForgeryManager.SetCookie();
        return await ApplicationConfigurationAppService.GetAsync(options);
    }
}
