using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using Volo.Abp.Auditing;
using Volo.Abp.Localization;

namespace Volo.Abp.AspNetCore.Mvc.Localization;

[Area("Abp")]
[Route("Abp/LocalizationScript")]
[DisableAuditing]
[RemoteService(false)]
[ApiExplorerSettings(IgnoreApi = true)]
public class AbpLocalizationScriptController
{
    protected AbpLocalizationOptions LocalizationOptions { get; }
    protected IStringLocalizerFactory StringLocalizerFactory { get; }

    public AbpLocalizationScriptController(
        IOptions<AbpLocalizationOptions> localizationOptions, 
        IStringLocalizerFactory stringLocalizerFactory)
    {
        StringLocalizerFactory = stringLocalizerFactory;
        LocalizationOptions = localizationOptions.Value;
    }
    
    [HttpGet]
    [Route("{culture}")]
    public async Task GetAsync(string culture)
    {
        // TODO: Should not get dynamic overloads, but how? What if we do? (Can be switched to host?)
        using (CultureHelper.Use(culture))
        {
            foreach (var resource in LocalizationOptions.Resources.Values)
            {
                var localizer = StringLocalizerFactory.CreateByResourceNameOrNull(resource.ResourceName);
                localizer?.GetAllStrings();
            }
        }
    }
}