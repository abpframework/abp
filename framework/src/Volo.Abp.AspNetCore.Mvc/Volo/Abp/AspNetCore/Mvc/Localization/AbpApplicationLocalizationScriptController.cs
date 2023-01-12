using System;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using Volo.Abp.AspNetCore.Mvc.ApplicationConfigurations;
using Volo.Abp.Auditing;
using Volo.Abp.Http;
using Volo.Abp.Json;
using Volo.Abp.Localization;
using Volo.Abp.Minify.Scripts;

namespace Volo.Abp.AspNetCore.Mvc.Localization;

[Area("Abp")]
[Route("Abp/ApplicationLocalizationScript")]
[DisableAuditing]
[RemoteService(false)]
[ApiExplorerSettings(IgnoreApi = true)]
public class AbpApplicationLocalizationScriptController : AbpController
{
    protected AbpApplicationLocalizationAppService LocalizationAppService { get; }
    protected AbpAspNetCoreMvcOptions Options { get; }
    protected IJsonSerializer JsonSerializer { get; }
    protected IJavascriptMinifier JavascriptMinifier { get; }

    public AbpApplicationLocalizationScriptController(
        AbpApplicationLocalizationAppService localizationAppService,
        IOptions<AbpAspNetCoreMvcOptions> options,
        IJsonSerializer jsonSerializer,
        IJavascriptMinifier javascriptMinifier)
    {
        LocalizationAppService = localizationAppService;
        JsonSerializer = jsonSerializer;
        JavascriptMinifier = javascriptMinifier;
        Options = options.Value;
    }

    [HttpGet]
    [Produces(MimeTypes.Application.Javascript, MimeTypes.Text.Plain)]
    public async Task<ActionResult> GetAsync(ApplicationLocalizationRequestDto input)
    {
        var script = CreateScript(
            await LocalizationAppService.GetAsync(input)
        );

        return Content(
            Options.MinifyGeneratedScript == true
                ? JavascriptMinifier.Minify(script)
                : script,
            MimeTypes.Application.Javascript
        );
    }

    private string CreateScript(ApplicationLocalizationDto localizationDto)
    {
        var script = new StringBuilder();

        script.AppendLine("(function(){");
        script.AppendLine();
        script.AppendLine(
            $"$.extend(true, abp.localization, {JsonSerializer.Serialize(localizationDto, indented: true)})");
        script.AppendLine();
        script.Append("})();");

        return script.ToString();
    }
}