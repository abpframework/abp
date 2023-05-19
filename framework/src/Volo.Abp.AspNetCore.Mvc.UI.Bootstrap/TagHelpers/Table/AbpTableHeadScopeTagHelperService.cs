using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Table;

public class AbpTableHeadScopeTagHelperService : AbpTagHelperService<AbpTableHeadScopeTagHelper>
{
    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        SetScope(context, output);
    }

    protected virtual void SetScope(TagHelperContext context, TagHelperOutput output)
    {
        switch (TagHelper.Scope)
        {
            case AbpThScope.Default:
                return;
            case AbpThScope.Row:
                output.Attributes.Add("scope", "row");
                return;
            case AbpThScope.Column:
                output.Attributes.Add("scope", "col");
                return;
        }
    }
}
