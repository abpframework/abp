using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Utils
{
    [HtmlTargetElement(Attributes = "abp-if")]
    public class AbpIfTagHelper : AbpTagHelper
    {
        [HtmlAttributeName("abp-if")]
        public bool Condition { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if (!Condition)
            {
                output.SuppressOutput();
            }
        }
    }
}
