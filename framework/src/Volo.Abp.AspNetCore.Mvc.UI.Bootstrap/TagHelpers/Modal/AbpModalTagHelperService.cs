using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Text;
using System.Threading.Tasks;

namespace Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Modal
{
    public class AbpModalTagHelperService : AbpTagHelperService<AbpModalTagHelper>
    {
        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = null;

            var childContent = await output.GetChildContentAsync();

            SetContent(context, output, childContent);
        }

        protected virtual void SetContent(TagHelperContext context, TagHelperOutput output, TagHelperContent childContent)
        {
            var modalContent = GetModalContentElement(context, output, childContent);
            var modalDialog = GetModalDialogElement(context, output, modalContent);
            var modal = GetModal(context, output, modalDialog);

            output.Content.SetHtmlContent(modal);
        }

        protected virtual TagBuilder GetModalContentElement(TagHelperContext context, TagHelperOutput output, TagHelperContent childContent)
        {
            var element = new TagBuilder("div");
            element.AddCssClass(GetModalContentClasses());
            element.InnerHtml.SetHtmlContent(childContent);
            return element;
        }

        protected virtual TagBuilder GetModalDialogElement(TagHelperContext context, TagHelperOutput output, IHtmlContent innerHtml)
        {
            var element = new TagBuilder("div");
            element.AddCssClass(GetModalDialogClasses());
            element.Attributes.Add("role", "document");
            element.InnerHtml.SetHtmlContent(innerHtml);
            return element;
        }

        protected virtual TagBuilder GetModal(TagHelperContext context, TagHelperOutput output, IHtmlContent innerHtml)
        {
            var element = new TagBuilder("div");
            element.AddCssClass(GetModalClasses());
            element.Attributes.Add("tabindex", "-1");
            element.Attributes.Add("role", "dialog");
            element.Attributes.Add("aria-hidden", "true");

            foreach (var attr in output.Attributes)
            {
                element.Attributes.Add(attr.Name, attr.Value.ToString());
            }

            SetDataAttributes(element);

            element.InnerHtml.SetHtmlContent(innerHtml);

            return element;
        }

        protected virtual string GetModalClasses()
        {
            return "modal fade";
        }

        protected virtual string GetModalDialogClasses()
        {
            var classNames = new StringBuilder("modal-dialog");

            if (TagHelper.Centered ?? false)
            {
                classNames.Append(" ");
                classNames.Append("modal-dialog-centered");
            }

            if (TagHelper.Scrollable ?? false)
            {
                classNames.Append(" ");
                classNames.Append("modal-dialog-scrollable");
            }

            if (TagHelper.Size != AbpModalSize.Default)
            {
                classNames.Append(" ");
                classNames.Append(TagHelper.Size.ToClassName());
            }

            return classNames.ToString();
        }

        protected virtual string GetModalContentClasses()
        {
            return "modal-content";
        }

        protected virtual string GetDataAttributes()
        {
            if (TagHelper.Static == true)
            {
                return "data-backdrop=\"static\" ";
            }
            return string.Empty;
        }

        protected virtual void SetDataAttributes(TagBuilder builder)
        {
            if (TagHelper.Static == true)
            {
                builder.Attributes.Add("data-backdrop", "static");
            }
        }
    }
}
