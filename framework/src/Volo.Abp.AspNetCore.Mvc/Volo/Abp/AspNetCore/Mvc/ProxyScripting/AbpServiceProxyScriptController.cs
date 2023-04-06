using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Volo.Abp.AspNetCore.Mvc.ControllerScriptCacheItem;
using Volo.Abp.Auditing;
using Volo.Abp.Caching;
using Volo.Abp.Http;
using Volo.Abp.Http.ProxyScripting;
using Volo.Abp.Minify.Scripts;

namespace Volo.Abp.AspNetCore.Mvc.ProxyScripting;

[Area("Abp")]
[Route("Abp/ServiceProxyScript")]
[DisableAuditing]
[RemoteService(false)]
[ApiExplorerSettings(IgnoreApi = true)]
public class AbpServiceProxyScriptController : AbpController
{
    protected readonly IProxyScriptManager ProxyScriptManager;
    protected readonly AbpAspNetCoreMvcOptions Options;
    protected readonly IJavascriptMinifier JavascriptMinifier;
    protected readonly IDistributedCache<AbpControllerScriptCacheItem> ScriptCache;

    public AbpServiceProxyScriptController(
        IProxyScriptManager proxyScriptManager,
        IOptions<AbpAspNetCoreMvcOptions> options,
        IJavascriptMinifier javascriptMinifier,
        IDistributedCache<AbpControllerScriptCacheItem> scriptCache)
    {
        ProxyScriptManager = proxyScriptManager;
        Options = options.Value;
        JavascriptMinifier = javascriptMinifier;
        ScriptCache = scriptCache;
    }

    [HttpGet]
    [Produces(MimeTypes.Application.Javascript, MimeTypes.Text.Plain)]
    public virtual async Task<ActionResult> GetAll(ServiceProxyGenerationModel model)
    {
        var script = await GetScript(model);

        return Content(
            Options.MinifyGeneratedScript == true
                ? JavascriptMinifier.Minify(script)
                : script,
            MimeTypes.Application.Javascript
        );
    }

    protected virtual async Task<string> GetScript(ServiceProxyGenerationModel model)
    {
        var script = (await ScriptCache.GetAsync(nameof(AbpServiceProxyScriptController)))?.Script;
        if (!script.IsNullOrWhiteSpace())
        {
            return script;
        }

        model.Normalize();
        return ProxyScriptManager.GetScript(model.CreateOptions());
    }
}
