using System;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Volo.Abp.AspNetCore.Mvc.AntiForgery;
using Volo.Abp.AspNetCore.Mvc.ControllerScriptCacheItem;
using Volo.Abp.Auditing;
using Volo.Abp.Caching;
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
    public const string GetCacheProfileName = $"{nameof(AbpApplicationConfigurationScriptController)}.{nameof(AbpApplicationConfigurationScriptController.Get)}";

    protected readonly AbpApplicationConfigurationAppService ConfigurationAppService;
    protected readonly IJsonSerializer JsonSerializer;
    protected readonly AbpAspNetCoreMvcOptions Options;
    protected readonly IJavascriptMinifier JavascriptMinifier;
    protected readonly IAbpAntiForgeryManager AntiForgeryManager;
    protected readonly IDistributedCache<AbpControllerScriptCacheItem> ScriptCache;

    public AbpApplicationConfigurationScriptController(
        AbpApplicationConfigurationAppService configurationAppService,
        IJsonSerializer jsonSerializer,
        IOptions<AbpAspNetCoreMvcOptions> options,
        IJavascriptMinifier javascriptMinifier,
        IAbpAntiForgeryManager antiForgeryManager,
        IDistributedCache<AbpControllerScriptCacheItem> scriptCache)
    {
        ConfigurationAppService = configurationAppService;
        JsonSerializer = jsonSerializer;
        Options = options.Value;
        JavascriptMinifier = javascriptMinifier;
        AntiForgeryManager = antiForgeryManager;
        ScriptCache = scriptCache;
    }

    [HttpGet]
    [ResponseCache(CacheProfileName = GetCacheProfileName)]
    [Produces(MimeTypes.Application.Javascript, MimeTypes.Text.Plain)]
    public virtual async Task<ActionResult> Get()
    {
        AntiForgeryManager.SetCookie();

        var script = await CreateAbpExtendScript();

        return Content(
            Options.MinifyGeneratedScript == true
                ? JavascriptMinifier.Minify(script)
                : script,
            MimeTypes.Application.Javascript
        );
    }

    protected virtual async Task<string> CreateAbpExtendScript()
    {
        var json = (await ScriptCache.GetAsync(nameof(AbpApplicationConfigurationScriptController)))?.Script;
        if (json.IsNullOrWhiteSpace())
        {
            json = JsonSerializer.Serialize(await ConfigurationAppService.GetAsync(new ApplicationConfigurationRequestOptions
            {
                IncludeLocalizationResources = false
            }), indented: true);
        }

        var script = new StringBuilder();

        script.AppendLine("(function(){");
        script.AppendLine();
        script.AppendLine($"$.extend(true, abp, {json})");
        script.AppendLine();
        script.AppendLine("abp.event.trigger('abp.configurationInitialized');");
        script.AppendLine();
        script.Append("})();");

        return script.ToString();
    }
}
