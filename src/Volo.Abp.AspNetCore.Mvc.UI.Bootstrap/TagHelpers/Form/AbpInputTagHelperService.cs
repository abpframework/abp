using System.Linq;
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
    public class AbpInputTagHelperService : AbpTagHelperService<AbpInputTagHelper>
    {
        private readonly IHtmlGenerator _generator;

        public AbpInputTagHelperService(IHtmlGenerator generator)
        {
            _generator = generator;
        }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "input";

            ProcessAttributes(output);
        }

        protected virtual void ProcessAttributes(TagHelperOutput output)
        {
            output.Attributes.RemoveAll("asp-for");

            var inputTagHelperOutput = GetAttributes(output);

            foreach (var tagHelperAttribute in inputTagHelperOutput.Attributes)
            {
                output.Attributes.Add(tagHelperAttribute);
            }

            output.Attributes.Add("class", "form-control");
        }

        protected virtual TagHelperOutput GetAttributes(TagHelperOutput output)
        {
            var inputTagHelper = new InputTagHelper(_generator)
            {
                For = TagHelper.AspFor,
                ViewContext = TagHelper.ViewContext
            };

            return GetInnerTagHelper(new TagHelperAttributeList(), inputTagHelper);
        }
    }
}