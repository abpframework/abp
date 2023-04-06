using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.CacheScripts;

[HtmlTargetElement("abp-application-localization-script", TagStructure = TagStructure.NormalOrSelfClosing)]
public class AbpApplicationLocalizationScriptTagHelper : AbpTagHelper<AbpApplicationLocalizationScriptTagHelper, AbpApplicationLocalizationScriptTagHelperService>
{
    [HtmlAttributeName("defer")]
    public bool Defer { get; set; }

    [HtmlAttributeName("culture-name")]
    public string CultureName { get; set; }

    [HtmlAttributeName("only-dynamics")]
    public bool OnlyDynamics { get; set; }

    public AbpApplicationLocalizationScriptTagHelper(AbpApplicationLocalizationScriptTagHelperService service)
        : base(service)
    {

    }
}
