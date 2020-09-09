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

            Process(context, output, childContent);
        }

        protected virtual void Process(TagHelperContext context, TagHelperOutput output, TagHelperContent content)
        {
            var modalContent = new TagBuilder("div");
            modalContent.AddCssClass(GetModalContentClasses());
            modalContent.InnerHtml.AppendHtml(content);

            var modalDialog = new TagBuilder("div");
            modalDialog.AddCssClass(GetModalDialogClasses());
            modalDialog.Attributes.Add("role", "document");
            modalDialog.InnerHtml.AppendHtml(modalContent);

            var modal = new TagBuilder("div");
            modal.AddCssClass(GetModalClasses());
            modal.Attributes.Add("tabindex", "-1");
            modal.Attributes.Add("role", "dialog");
            modal.Attributes.Add("aria-hidden", "true");

            foreach (var attr in output.Attributes)
            {
                modal.Attributes.Add(attr.Name, attr.Value.ToString());
            }

            if (TagHelper.Static == true)
            {
                modal.Attributes.Add("data-backdrop", "static");
            }

            modal.InnerHtml.AppendHtml(modalDialog);

            output.Content.SetHtmlContent(modal);
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
    }
}