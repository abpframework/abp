using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.AntiForgery;

namespace Volo.Abp.AspNetCore.Mvc.ApplicationConfigurations;

[Area("abp")]
[RemoteService(Name = "abp")]
[Route("api/abp/application-configuration")]
public class AbpApplicationConfigurationController : AbpControllerBase, IAbpApplicationConfigurationAppService
{
    public const string CacheProfileName = $"{nameof(AbpApplicationConfigurationController)}.{nameof(AbpApplicationConfigurationController.GetAsync)}";

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
    [ResponseCache(CacheProfileName = CacheProfileName)]
    public virtual async Task<ApplicationConfigurationDto> GetAsync(
        ApplicationConfigurationRequestOptions options)
    {
        AntiForgeryManager.SetCookie();
        return await ApplicationConfigurationAppService.GetAsync(options);
    }
}
