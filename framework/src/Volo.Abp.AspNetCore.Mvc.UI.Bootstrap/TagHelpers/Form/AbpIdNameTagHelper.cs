using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.Options;

namespace Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form
{
    [HtmlTargetElement(Attributes = "abp-id-name")]
    public class AbpIdNameTagHelper : AbpTagHelper
    {
        /// <summary>
        /// Make sure this TagHelper is executed first.
        /// </summary>
        public override int Order => -1000 - 1;

        [HtmlAttributeName("abp-id-name")]
        public ModelExpression IdNameFor { get; set; }

        private readonly MvcViewOptions _mvcViewOptions;

        public AbpIdNameTagHelper(IOptions<MvcViewOptions> mvcViewOptions)
        {
            _mvcViewOptions = mvcViewOptions.Value;
        }

        public override Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            if (IdNameFor != null)
            {
                if (!context.AllAttributes.Any(x => x.Name.Equals("id", StringComparison.OrdinalIgnoreCase)))
                {
                    var id = TagBuilder.CreateSanitizedId(IdNameFor.Name, _mvcViewOptions.HtmlHelperOptions.IdAttributeDotReplacement);
                    output.Attributes.Add("id", id);
                }

                if (!context.AllAttributes.Any(x => x.Name.Equals("name", StringComparison.OrdinalIgnoreCase)))
                {
                    output.Attributes.Add("name", IdNameFor.Name);
                }
            }

            return Task.CompletedTask;
        }
    }
}
