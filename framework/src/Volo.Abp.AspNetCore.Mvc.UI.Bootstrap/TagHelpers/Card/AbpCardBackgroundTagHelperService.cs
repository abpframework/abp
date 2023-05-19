using Microsoft.AspNetCore.Razor.TagHelpers;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.Microsoft.AspNetCore.Razor.TagHelpers;

namespace Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Card;

public class AbpCardBackgroundTagHelperService : AbpTagHelperService<AbpCardBackgroundTagHelper>
{
    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        SetBackground(context, output);
    }

    protected virtual void SetBackground(TagHelperContext context, TagHelperOutput output)
    {
        if (TagHelper.Background == AbpCardBackgroundType.Default)
        {
            return;
        }

        output.Attributes.AddClass("bg-" + TagHelper.Background.ToString().ToLowerInvariant());
    }
}
