using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Button
{
    [HtmlTargetElement("abp-button", TagStructure = TagStructure.NormalOrSelfClosing)]
    public class AbpButtonTagHelper : AbpTagHelper<AbpButtonTagHelper, AbpButtonTagHelperService>
    {
        public AbpButtonType ButtonType { get; set; } = AbpButtonType.Default;

        public string BusyText { get; set; }

        public string Text { get; set; }

        public string Icon { get; set; }

        public FontIconType IconType { get; set; } = FontIconType.FontAwesome;

        public AbpButtonTagHelper(AbpButtonTagHelperService service) 
            : base(service)
        {

        }
    }
}
