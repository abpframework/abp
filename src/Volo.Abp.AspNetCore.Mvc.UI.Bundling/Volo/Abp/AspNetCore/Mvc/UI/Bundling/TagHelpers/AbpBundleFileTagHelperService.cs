using System.Collections.Generic;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers;

namespace Volo.Abp.AspNetCore.Mvc.UI.Bundling.TagHelpers
{
    public class AbpBundleFileTagHelperService : AbpTagHelperService<AbpBundleFileTagHelper>
    {
        public const string ContextFileListKey = "AbpBundleFileTagHelperService.BundleFiles";

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.SuppressOutput();

            var files = (List<string>)context.Items[ContextFileListKey];
            files.Add(TagHelper.Src);
        }
    }
}