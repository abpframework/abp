using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers;

namespace Volo.CmsKit.Web.Icons
{
    public class CmsIconTagHelperService : AbpTagHelperService<CmsIconTagHelper>
    {
        public override Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            if (TagHelper.Name.Contains("."))
            {
                output.TagName = "img";
                output.Attributes.Add("src", TagHelper.Name);
                output.Attributes.Add("width", "20");
                output.Attributes.Add("height", "20");
            }
            else
            {
                //TODO: Allow to font, svg icons.. etc.
                throw new AbpException("Only file icons are allowed!");
            }

            if (TagHelper.Highlight)
            {
                output.AddClass("cms-icon-highlighted", HtmlEncoder.Default);
            }

            return Task.CompletedTask;
        }
    }
}
