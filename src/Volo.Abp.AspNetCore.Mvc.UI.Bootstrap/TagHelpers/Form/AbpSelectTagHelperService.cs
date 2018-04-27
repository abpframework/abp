using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.Microsoft.AspNetCore.Razor.TagHelpers;

namespace Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form
{
    public class AbpSelectTagHelperService : AbpTagHelperService<AbpSelectTagHelper>
    {
        private readonly IHtmlGenerator _generator;
        private readonly HtmlEncoder _encoder;

        public AbpSelectTagHelperService(IHtmlGenerator generator, HtmlEncoder encoder)
        {
            _generator = generator;
            _encoder = encoder;
        }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "div";
            output.Attributes.Add("class", "form-control");
            output.TagMode = TagMode.StartTagAndEndTag;

            output.Content.SetHtmlContent(GetInnerHtml(output));
            output.Attributes.Clear();
        }

        protected virtual string GetInnerHtml(TagHelperOutput output)
        {
            var selectTagHelper = new SelectTagHelper(_generator)
            {
                For = TagHelper.AspFor,
                Items = TagHelper.AspItems,
                ViewContext = TagHelper.ViewContext
            };

            return RenderTagHelper(output.Attributes, selectTagHelper, _encoder, "select", TagMode.StartTagAndEndTag);
        }
    }
}