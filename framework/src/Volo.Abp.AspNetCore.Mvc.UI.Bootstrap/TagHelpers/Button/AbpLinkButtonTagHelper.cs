using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Button;

[HtmlTargetElement("a", Attributes = "abp-button", TagStructure = TagStructure.NormalOrSelfClosing)]
[HtmlTargetElement("input", Attributes = "abp-button", TagStructure = TagStructure.WithoutEndTag)]
public class AbpLinkButtonTagHelper : AbpTagHelper<AbpLinkButtonTagHelper, AbpLinkButtonTagHelperService>, IButtonTagHelperBase
{
    [HtmlAttributeName("abp-button")]
    public AbpButtonType ButtonType { get; set; }

    public AbpButtonSize Size { get; set; } = AbpButtonSize.Default;

    public string Text { get; set; }

    public string Icon { get; set; }

    public bool? Disabled { get; set; }

    public FontIconType IconType { get; } = FontIconType.FontAwesome;

    public AbpLinkButtonTagHelper(AbpLinkButtonTagHelperService service)
        : base(service)
    {

    }
}
