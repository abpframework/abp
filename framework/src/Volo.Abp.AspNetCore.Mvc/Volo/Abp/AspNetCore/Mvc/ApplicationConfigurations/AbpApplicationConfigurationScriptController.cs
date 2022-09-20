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
    private readonly IAbpApplicationConfigurationAppService _configurationAppService;
    private readonly IJsonSerializer _jsonSerializer;
    private readonly AbpAspNetCoreMvcOptions _options;
    private readonly IJavascriptMinifier _javascriptMinifier;
    private readonly IAbpAntiForgeryManager _antiForgeryManager;

    public AbpApplicationConfigurationScriptController(
        IAbpApplicationConfigurationAppService configurationAppService,
        IJsonSerializer jsonSerializer,
        IOptions<AbpAspNetCoreMvcOptions> options,
        IJavascriptMinifier javascriptMinifier,
        IAbpAntiForgeryManager antiForgeryManager)
    {
        _configurationAppService = configurationAppService;
        _jsonSerializer = jsonSerializer;
        _options = options.Value;
        _javascriptMinifier = javascriptMinifier;
        _antiForgeryManager = antiForgeryManager;
    }

    [HttpGet]
    [Produces(MimeTypes.Application.Javascript, MimeTypes.Text.Plain)]
    public async Task<ActionResult> Get()
    {
        var script = CreateAbpExtendScript(await _configurationAppService.GetAsync());

        _antiForgeryManager.SetCookie();

        return Content(
            _options.MinifyGeneratedScript == true
                ? _javascriptMinifier.Minify(script)
                : script,
            MimeTypes.Application.Javascript
        );
    }

    private string CreateAbpExtendScript(ApplicationConfigurationDto config)
    {
        var script = new StringBuilder();

        script.AppendLine("(function(){");
        script.AppendLine();
        script.AppendLine($"$.extend(true, abp, {_jsonSerializer.Serialize(config, indented: true)})");
        script.AppendLine();
        script.AppendLine("abp.event.trigger('abp.configurationInitialized');");
        script.AppendLine();
        script.Append("})();");

        return script.ToString();
    }
}
