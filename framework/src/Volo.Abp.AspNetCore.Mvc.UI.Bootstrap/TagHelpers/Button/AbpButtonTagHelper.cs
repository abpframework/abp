using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Button
{
    [HtmlTargetElement("abp-button", TagStructure = TagStructure.NormalOrSelfClosing)]
    public class AbpButtonTagHelper : AbpTagHelper<AbpButtonTagHelper, AbpButtonTagHelperService>, IButtonTagHelperBase
    {
        public AbpButtonType ButtonType { get; set; } = AbpButtonType.Default;

        public AbpButtonSize Size { get; set; } = AbpButtonSize.Default;

        public string BusyText { get; set; }

        public string Text { get; set; }

        public string Icon { get; set; }

        public bool? Disabled { get; set; }

        public FontIconType IconType { get; set; } = FontIconType.FontAwesome;

        public bool BusyTextIsHtml { get; set; }

        public AbpButtonTagHelper(AbpButtonTagHelperService service) 
            : base(service)
        {

        }
    }
}

