using Microsoft.AspNetCore.Razor.TagHelpers;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.Microsoft.AspNetCore.Razor.TagHelpers;

namespace Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Table
{
    public class AbpTableHeaderTagHelperService : AbpTagHelperService<AbpTableHeaderTagHelper>
    {
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            SetTheme(context, output);
        }

        protected virtual void SetTheme(TagHelperContext context, TagHelperOutput output)
        {
            switch (TagHelper.Theme)
            {
                case AbpTableHeaderTheme.Default:
                    return;
                case AbpTableHeaderTheme.Dark:
                    output.Attributes.AddClass("thead-dark");
                    return;
                case AbpTableHeaderTheme.Light:
                    output.Attributes.AddClass("thead-light");
                    return;
            }
        }
    }
}