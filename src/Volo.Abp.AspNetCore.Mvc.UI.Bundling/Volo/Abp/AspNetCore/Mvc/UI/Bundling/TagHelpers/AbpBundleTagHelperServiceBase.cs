using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers;

namespace Volo.Abp.AspNetCore.Mvc.UI.Bundling.TagHelpers
{
    public abstract class AbpBundleTagHelperServiceBase<TTagHelper> : AbpTagHelperService<TTagHelper>
        where TTagHelper : TagHelper, IBundleTagHelper
    {
        protected AbpTagHelperResourceHelper ResourceHelper { get; }

        protected AbpBundleTagHelperServiceBase(AbpTagHelperResourceHelper resourceHelper)
        {
            ResourceHelper = resourceHelper;
        }

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            await ResourceHelper.ProcessAsync(
                context,
                output,
                TagHelper.GetNameOrNull(),
                await GetBundleItems(context, output)
            );
        }

        protected virtual async Task<List<BundleTagHelperItem>> GetBundleItems(TagHelperContext context, TagHelperOutput output)
        {
            var fileList = new List<BundleTagHelperItem>();
            context.Items[AbpTagHelperConsts.ContextBundleItemListKey] = fileList;
            await output.GetChildContentAsync(); //TODO: Suppress child execution!
            return fileList;
        }
    }
}