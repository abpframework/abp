using Microsoft.AspNetCore.Razor.TagHelpers;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.Microsoft.AspNetCore.Razor.TagHelpers;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Button;

namespace Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Collapse;

public class AbpCollapseButtonTagHelperService : AbpTagHelperService<AbpCollapseButtonTagHelper>
{
    public override void Process(TagHelperContext context, TagHelperOutput output)
    {


        AddCommonAttributes(context, output);

        if (output.TagName == "abp-button" || output.TagName == "button")
        {
            AddButtonAttributes(context, output);
        }
        else if (output.TagName == "a")
        {
            AddLinkAttributes(context, output);
        }
    }

    protected virtual void AddCommonAttributes(TagHelperContext context, TagHelperOutput output)
    {
        output.Attributes.Add("data-bs-toggle", "collapse");
        output.Attributes.Add("aria-expanded", "false");
        output.Attributes.Add("aria-controls", TagHelper.BodyId);
    }

    protected virtual void AddButtonAttributes(TagHelperContext context, TagHelperOutput output)
    {
        if (TagHelper.BodyId.Trim().Split(' ').Length > 1)
        {
            output.Attributes.Add("data-bs-target", ".multi-collapse");
            return;
        }

        output.Attributes.Add("data-bs-target", "#" + TagHelper.BodyId);
    }

    protected virtual void AddLinkAttributes(TagHelperContext context, TagHelperOutput output)
    {
        if (TagHelper.BodyId.Trim().Split(' ').Length > 1)
        {
            output.Attributes.Add("href", ".multi-collapse");
            return;
        }

        output.Attributes.Add("href", "#" + TagHelper.BodyId);
    }

}
