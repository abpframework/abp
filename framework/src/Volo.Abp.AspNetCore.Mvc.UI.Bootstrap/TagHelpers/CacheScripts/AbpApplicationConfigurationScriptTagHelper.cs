using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.CacheScripts;

[HtmlTargetElement("abp-application-configuration-script", TagStructure = TagStructure.NormalOrSelfClosing)]
public class AbpApplicationConfigurationScriptTagHelper : AbpTagHelper<AbpApplicationConfigurationScriptTagHelper, AbpApplicationConfigurationScriptTagHelperService>
{
    [HtmlAttributeName("defer")]
    public bool Defer { get; set; }

    [HtmlAttributeName("include-localization-resources")]
    public bool IncludeLocalizationResources { get; set; }

    public AbpApplicationConfigurationScriptTagHelper(AbpApplicationConfigurationScriptTagHelperService service)
        : base(service)
    {

    }
}
