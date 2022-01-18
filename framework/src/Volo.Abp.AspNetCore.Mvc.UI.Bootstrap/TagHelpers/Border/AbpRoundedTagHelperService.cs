using Microsoft.AspNetCore.Razor.TagHelpers;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.Microsoft.AspNetCore.Razor.TagHelpers;

namespace Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Border;

public class AbpRoundedTagHelperService : AbpTagHelperService<AbpRoundedTagHelper>
{
    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        var roundedClass = "rounded";

        if (TagHelper.AbpRounded != AbpRoundedType.Default)
        {
            roundedClass += "-" + TagHelper.AbpRounded.ToString().ToLowerInvariant().Replace("_", "");
        }

        output.Attributes.AddClass(roundedClass);
    }
}
