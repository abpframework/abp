using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Volo.Abp.AspNetCore.Mvc.AntiForgery;
using Volo.Abp.Auditing;
using Volo.Abp.Http;
using Volo.Abp.Json;
using Volo.Abp.Minify.Scripts;

namespace Volo.Abp.AspNetCore.Mvc.ApplicationConfigurations;

[Area("Abp")]
[Route("Abp/ApplicationConfigurationScript")]
[DisableAuditing]
[RemoteService(false)]
[ApiExplorerSettings(IgnoreApi = true)]
public class AbpApplicationConfigurationScriptController : AbpController
{
    protected readonly AbpApplicationConfigurationAppService ConfigurationAppService;
    protected readonly IJsonSerializer JsonSerializer;
    protected readonly AbpAspNetCoreMvcOptions Options;
    protected readonly IJavascriptMinifier JavascriptMinifier;
    protected readonly IAbpAntiForgeryManager AntiForgeryManager;

    public AbpApplicationConfigurationScriptController(
        AbpApplicationConfigurationAppService configurationAppService,
        IJsonSerializer jsonSerializer,
        IOptions<AbpAspNetCoreMvcOptions> options,
        IJavascriptMinifier javascriptMinifier,
        IAbpAntiForgeryManager antiForgeryManager)
    {
        ConfigurationAppService = configurationAppService;
        JsonSerializer = jsonSerializer;
        Options = options.Value;
        JavascriptMinifier = javascriptMinifier;
        AntiForgeryManager = antiForgeryManager;
    }

    [HttpGet]
    [Produces(MimeTypes.Application.Javascript, MimeTypes.Text.Plain)]
    public virtual async Task<ActionResult> Get()
    {
        var script = CreateAbpExtendScript(
            await ConfigurationAppService.GetAsync(
                new ApplicationConfigurationRequestOptions {
                    IncludeLocalizationResources = false
                }
            )
        );

        AntiForgeryManager.SetCookie();

        return Content(
            Options.MinifyGeneratedScript == true
                ? JavascriptMinifier.Minify(script)
                : script,
            MimeTypes.Application.Javascript
        );
    }

    protected virtual string CreateAbpExtendScript(ApplicationConfigurationDto config)
    {
        var script = new StringBuilder();

        script.AppendLine("(function(){");
        script.AppendLine();
        script.AppendLine($"$.extend(true, abp, {JsonSerializer.Serialize(config, indented: true)})");
        script.AppendLine();
        script.AppendLine("abp.event.trigger('abp.configurationInitialized');");
        script.AppendLine();
        script.Append("})();");

        return script.ToString();
    }
}
