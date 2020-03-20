using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Modal
{
    [HtmlTargetElement("abp-modal-footer")]
    public class AbpModalFooterTagHelper : AbpTagHelper<AbpModalFooterTagHelper, AbpModalFooterTagHelperService>
    {
        public AbpModalButtons Buttons { get; set; }
        public ButtonsAlign ButtonAlignment { get; set; } = ButtonsAlign.Default;

        public AbpModalFooterTagHelper(AbpModalFooterTagHelperService tagHelperService)
            : base(tagHelperService)
        {
        }
    }
}