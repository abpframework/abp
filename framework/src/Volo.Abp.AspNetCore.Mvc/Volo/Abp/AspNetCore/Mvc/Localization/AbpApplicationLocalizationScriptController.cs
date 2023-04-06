using System;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Volo.Abp.AspNetCore.Mvc.ApplicationConfigurations;
using Volo.Abp.AspNetCore.Mvc.ControllerScriptCacheItem;
using Volo.Abp.Auditing;
using Volo.Abp.Caching;
using Volo.Abp.Http;
using Volo.Abp.Json;
using Volo.Abp.Minify.Scripts;

namespace Volo.Abp.AspNetCore.Mvc.Localization;

[Area("Abp")]
[Route("Abp/ApplicationLocalizationScript")]
[DisableAuditing]
[RemoteService(false)]
[ApiExplorerSettings(IgnoreApi = true)]
public class AbpApplicationLocalizationScriptController : AbpController
{
    public const string GetCacheProfileName = $"{nameof(AbpApplicationLocalizationScriptController)}.{nameof(AbpApplicationLocalizationScriptController.GetAsync)}";

    protected AbpApplicationLocalizationAppService LocalizationAppService { get; }
    protected AbpAspNetCoreMvcOptions Options { get; }
    protected IJsonSerializer JsonSerializer { get; }
    protected IJavascriptMinifier JavascriptMinifier { get; }
    protected IDistributedCache<AbpControllerScriptCacheItem> ScriptCache { get; }

    public AbpApplicationLocalizationScriptController(
        AbpApplicationLocalizationAppService localizationAppService,
        IOptions<AbpAspNetCoreMvcOptions> options,
        IJsonSerializer jsonSerializer,
        IJavascriptMinifier javascriptMinifier,
        IDistributedCache<AbpControllerScriptCacheItem> scriptCache)
    {
        LocalizationAppService = localizationAppService;
        JsonSerializer = jsonSerializer;
        JavascriptMinifier = javascriptMinifier;
        ScriptCache = scriptCache;
        Options = options.Value;
    }

    [HttpGet]
    [ResponseCache(CacheProfileName = GetCacheProfileName)]
    [Produces(MimeTypes.Application.Javascript, MimeTypes.Text.Plain)]
    public virtual async Task<ActionResult> GetAsync(ApplicationLocalizationRequestDto input)
    {
        var script = await CreateScript(input);

        return Content(
            Options.MinifyGeneratedScript == true
                ? JavascriptMinifier.Minify(script)
                : script,
            MimeTypes.Application.Javascript
        );
    }

    protected virtual async Task<string> CreateScript(ApplicationLocalizationRequestDto input)
    {
        var json = (await ScriptCache.GetAsync(nameof(AbpApplicationLocalizationScriptController)))?.Script;
        if (json.IsNullOrWhiteSpace())
        {
            json = JsonSerializer.Serialize(await LocalizationAppService.GetAsync(input), indented: true);
        }

        var script = new StringBuilder();

        script.AppendLine("(function(){");
        script.AppendLine();
        script.AppendLine($"$.extend(true, abp.localization, {json})");
        script.AppendLine();
        script.Append("})();");

        return script.ToString();
    }
}
