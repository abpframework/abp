using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using Volo.Abp.AspNetCore.Mvc.ApplicationConfigurations;
using Volo.Abp.Auditing;
using Volo.Abp.Http;
using Volo.Abp.Localization;

namespace Volo.Abp.AspNetCore.Mvc.Localization;

[Area("Abp")]
[Route("Abp/ApplicationLocalizationScript")]
[DisableAuditing]
[RemoteService(false)]
[ApiExplorerSettings(IgnoreApi = true)]
public class AbpApplicationLocalizationScriptController : AbpController
{
   protected IAbpApplicationLocalizationAppService LocalizationAppService { get; }

    public AbpApplicationLocalizationScriptController(
        IAbpApplicationLocalizationAppService localizationAppService)
    {
        LocalizationAppService = localizationAppService;
    }
    
    [HttpGet]
    [Route("{culture}")]
    [Produces(MimeTypes.Application.Javascript, MimeTypes.Text.Plain)]
    public async Task GetAsync(string culture)
    {
        throw new NotImplementedException();
    }
}