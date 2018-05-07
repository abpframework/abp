using System.Linq;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Popover
{
    public class AbpPopoverTagHelperService : AbpTagHelperService<AbpPopoverTagHelper>
    {
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if (!TagHelper.Disabled??true)
            {
                SetDataToggle(context, output);
                SetDataPlacement(context, output);
                SetPopoverData(context, output);
                SetDataTriggerIfDismissible(context, output);
            }
            else
            {
                SetDisabled(context,output);
            }
        }

        protected virtual void SetDisabled(TagHelperContext context, TagHelperOutput output)
        {
            var triggerAsHtml = TagHelper.Dismissible ?? false ? "datatrigger=\"focus\" " : "";

            var dataPlacementAsHtml = "data-placement=\"" +GetDirectory().ToString().ToLowerInvariant() + "\" ";

            var titleAttribute = output.Attributes.FirstOrDefault(at => at.Name == "title");
            var titleAsHtml = titleAttribute == null? "":"title=\""+ titleAttribute.Value +"\" ";

            var preElementHtml = "<span class=\"d-inline-block\" "+ titleAsHtml + triggerAsHtml + dataPlacementAsHtml + "data-toggle=\"popover\" data-content=\"" +GetDataContent()+"\">";

            var postElementHtml = "</span>";

            output.PreElement.SetHtmlContent(preElementHtml);
            output.PostElement.SetHtmlContent(postElementHtml);

            output.Attributes.Add("style", "pointer-events: none;");
        }

        protected virtual void SetDataTriggerIfDismissible(TagHelperContext context, TagHelperOutput output)
        {
            if (TagHelper.Dismissible ?? false)
            {
                output.Attributes.Add("data-trigger", "focus");
            }
        }

        protected virtual void SetDataToggle(TagHelperContext context, TagHelperOutput output)
        {
            output.Attributes.Add("data-toggle", "popover");
        }

        protected virtual void SetDataPlacement(TagHelperContext context, TagHelperOutput output)
        {
            var directory = GetDirectory();
            if (directory == PopoverDirectory.Default)
            {
                directory = PopoverDirectory.Bottom;
            }
            output.Attributes.Add("data-placement", directory.ToString().ToLowerInvariant());
        }

        protected virtual void SetPopoverData(TagHelperContext context, TagHelperOutput output)
        {
            output.Attributes.Add("data-content", GetDataContent());
        }

        protected virtual string GetDataContent()
        {
            switch (GetDirectory())
            {
                case PopoverDirectory.Top:
                    return TagHelper.AbpPopoverTop;
                case PopoverDirectory.Right:
                    return TagHelper.AbpPopoverRight;
                case PopoverDirectory.Bottom:
                    return TagHelper.AbpPopoverBottom;
                case PopoverDirectory.Left:
                    return TagHelper.AbpPopoverLeft;
                default:
                    return TagHelper.AbpPopover;
            }
        }

        protected virtual PopoverDirectory GetDirectory()
        {
            if (!string.IsNullOrWhiteSpace(TagHelper.AbpPopoverTop))
            {
                return PopoverDirectory.Top;
            }
            if (!string.IsNullOrWhiteSpace(TagHelper.AbpPopoverBottom))
            {
                return PopoverDirectory.Bottom;
            }
            if (!string.IsNullOrWhiteSpace(TagHelper.AbpPopoverRight))
            {
                return PopoverDirectory.Right;
            }
            if (!string.IsNullOrWhiteSpace(TagHelper.AbpPopoverLeft))
            {
                return PopoverDirectory.Left;
            }

            return PopoverDirectory.Default;
        }
    }
}