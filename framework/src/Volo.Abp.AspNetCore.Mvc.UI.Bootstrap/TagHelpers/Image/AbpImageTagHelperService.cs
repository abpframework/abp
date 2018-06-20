using Microsoft.AspNetCore.Razor.TagHelpers;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.Microsoft.AspNetCore.Razor.TagHelpers;

namespace Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Image
{
    public class AbpImageTagHelperService : AbpTagHelperService<AbpImageTagHelper>
    {
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "img";
            SetSourceFile(context,output);
            SetPosition(context,output);
            SetResponsive(context,output);
            SetThumbnail(context,output);
            SetRounded(context,output);
            SetAlt(context,output);
        }
        
        protected virtual void SetSourceFile(TagHelperContext context, TagHelperOutput output) {
            output.Attributes.Add("src",TagHelper.Src);
        }
        
        protected virtual void SetPosition(TagHelperContext context, TagHelperOutput output)
        {
            if (TagHelper.Position == default)
            {
                return;
            }
            if (TagHelper.Position == AbpImagePosition.Left || TagHelper.Position == AbpImagePosition.Right)
            {
                output.Attributes.AddClass("float-" + TagHelper.Position.ToString().ToLowerInvariant());
            }
            if (TagHelper.Position == AbpImagePosition.Center)
            {
                output.PreElement.SetHtmlContent("<div class=\"text-center\">");
                output.PostElement.SetHtmlContent("</div>");
            }
        }
        
        protected virtual void SetResponsive(TagHelperContext context, TagHelperOutput output) {
            if (TagHelper.Responsive ?? false)
            {
                output.Attributes.AddClass("img-fluid");
            }
        }
        
        protected virtual void SetThumbnail(TagHelperContext context, TagHelperOutput output)
        {
            if (TagHelper.Thumbnail ?? false)
            {
                output.Attributes.AddClass("img-thumbnail");
            }
        }
        
        protected virtual void SetRounded(TagHelperContext context, TagHelperOutput output)
        {
            if (TagHelper.Rounded ?? false)
            {
                output.Attributes.AddClass("rounded");
            }
        }
        
        protected virtual void SetAlt(TagHelperContext context, TagHelperOutput output)
        {
            output.Attributes.Add("alt",TagHelper.Alt);
        }
    }
}