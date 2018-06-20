using Microsoft.AspNetCore.Razor.TagHelpers;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.Microsoft.AspNetCore.Razor.TagHelpers;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Button;

namespace Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Collapse
{
    public class AbpCollapseButtonTagHelperService : AbpTagHelperService<AbpCollapseButtonTagHelper>
    {
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "button";
            output.Attributes.AddClass("btn");
            output.Attributes.Add("data-toggle","collapse");
            output.Attributes.Add("aria-expanded","false");
            output.Attributes.Add("type","button");
            output.Attributes.Add("data-target", "#" +TagHelper.BodyId);
            output.Attributes.Add("aria-controls", TagHelper.BodyId);


            if (TagHelper.ButonType != AbpButtonType.Default)
            {
                output.Attributes.AddClass("btn-" + TagHelper.ButonType.ToString().ToLowerInvariant());
            }
        }
        
    }
}