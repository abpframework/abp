using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers;

namespace Volo.Abp.AspNetCore.Mvc.UI.Bundling.TagHelpers
{
    public abstract class AbpBundleTagHelperService<TTagHelper> : AbpTagHelperService<TTagHelper>
        where TTagHelper : TagHelper, IBundleTagHelper
    {
        protected AbpTagHelperResourceService ResourceService { get; }

        protected AbpBundleTagHelperService(AbpTagHelperResourceService resourceService)
        {
            ResourceService = resourceService;
        }

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            await ResourceService.ProcessAsync(
                context,
                output,
                await GetBundleItems(context, output).ConfigureAwait(false),
                TagHelper.GetNameOrNull()
            ).ConfigureAwait(false);
        }

        protected virtual async Task<List<BundleTagHelperItem>> GetBundleItems(TagHelperContext context, TagHelperOutput output)
        {
            var bundleItems = new List<BundleTagHelperItem>();
            context.Items[AbpTagHelperConsts.ContextBundleItemListKey] = bundleItems;
            await output.GetChildContentAsync().ConfigureAwait(false); //TODO: Is there a way of executing children without getting content?
            return bundleItems;
        }
    }
}