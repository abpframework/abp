using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.CacheScripts;

[HtmlTargetElement("abp-service-proxy-script", TagStructure = TagStructure.NormalOrSelfClosing)]
public class AbpServiceProxyScriptTagHelper : AbpTagHelper<AbpServiceProxyScriptTagHelper, AbpServiceProxyScriptTagHelperService>
{
    [HtmlAttributeName("defer")]
    public bool Defer { get; set; }

    [HtmlAttributeName("type")]
    public string Type { get; set; }

    [HtmlAttributeName("use-cache")]
    public bool UseCache { get; set; } = true;

    [HtmlAttributeName("modules")]
    public string Modules { get; set; }

    [HtmlAttributeName("controllers")]
    public string Controllers { get; set; }

    [HtmlAttributeName("actions")]
    public string Actions { get; set; }

    public AbpServiceProxyScriptTagHelper(AbpServiceProxyScriptTagHelperService service)
        : base(service)
    {

    }
}
