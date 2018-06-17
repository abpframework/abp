using System.Collections.Generic;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers;

namespace Volo.Abp.AspNetCore.Mvc.UI.Bundling.TagHelpers
{
    public class AbpBundleContributorTagHelperService : AbpTagHelperService<AbpBundleContributorTagHelper>
    {
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.SuppressOutput();

            var files = (List<BundleTagHelperItem>)context.Items[AbpTagHelperConsts.ContextBundleItemListKey];
            files.Add(new BundleTagHelperItem(TagHelper.Type));
        }
    }
}