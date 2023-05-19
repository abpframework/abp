using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Volo.Abp.Auditing;
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

    public AbpServiceProxyScriptController(IProxyScriptManager proxyScriptManager,
        IOptions<AbpAspNetCoreMvcOptions> options,
        IJavascriptMinifier javascriptMinifier)
    {
        ProxyScriptManager = proxyScriptManager;
        Options = options.Value;
        JavascriptMinifier = javascriptMinifier;
    }

    [HttpGet]
    [Produces(MimeTypes.Application.Javascript, MimeTypes.Text.Plain)]
    public virtual ActionResult GetAll(ServiceProxyGenerationModel model)
    {
        model.Normalize();

        var script = ProxyScriptManager.GetScript(model.CreateOptions());

        return Content(
            Options.MinifyGeneratedScript == true
                ? JavascriptMinifier.Minify(script)
                : script,
            MimeTypes.Application.Javascript
        );
    }
}
