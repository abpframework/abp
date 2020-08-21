using System;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.Microsoft.AspNetCore.Razor.TagHelpers;

namespace Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Button
{
    public class AbpButtonGroupTagHelperService : AbpTagHelperService<AbpButtonGroupTagHelper>
    {
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            AddButtonGroupClass(context, output);
            AddSizeClass(context, output);
            AddAttributes(context, output);

            output.TagName = "div";
        }

        protected virtual void AddSizeClass(TagHelperContext context, TagHelperOutput output)
        {
            switch (TagHelper.Size)
            {
                case AbpButtonGroupSize.Default:
                    break;
                case AbpButtonGroupSize.Small:
                    output.Attributes.AddClass("btn-group-sm");
                    break;
                case AbpButtonGroupSize.Medium:
                    output.Attributes.AddClass("btn-group-md");
                    break;
                case AbpButtonGroupSize.Large:
                    output.Attributes.AddClass("btn-group-lg");
                    break;
            }
        }

        protected virtual void AddButtonGroupClass(TagHelperContext context, TagHelperOutput output)
        {
            switch (TagHelper.Direction)
            {
                case AbpButtonGroupDirection.Horizontal:
                    output.Attributes.AddClass("btn-group");
                    break;
                case AbpButtonGroupDirection.Vertical:
                    output.Attributes.AddClass("btn-group-vertical");
                    break;
                default:
                    output.Attributes.AddClass("btn-group");
                    break;
            }
        }

        protected virtual void AddAttributes(TagHelperContext context, TagHelperOutput output)
        {
            output.Attributes.Add("role", "group");
        }
    }
}
