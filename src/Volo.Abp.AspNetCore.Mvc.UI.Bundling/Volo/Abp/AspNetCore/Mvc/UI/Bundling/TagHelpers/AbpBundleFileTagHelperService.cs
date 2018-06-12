using System.Collections.Generic;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Bundling
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