using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers;

namespace Volo.Abp.AspNetCore.Mvc.UI.Bundling.TagHelpers
{
    public abstract class AbpTagHelperResourceItemService<TTagHelper> : AbpTagHelperService<TTagHelper>
        where TTagHelper : TagHelper, IBundleItemTagHelper
    {
        protected AbpTagHelperResourceService ResourceService { get; }

        protected AbpTagHelperResourceItemService(AbpTagHelperResourceService resourceService)
        {
            ResourceService = resourceService;
        }

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            var tagHelperItems = context.Items.GetOrDefault(AbpTagHelperConsts.ContextBundleItemListKey) as List<BundleTagHelperItem>;
            if (tagHelperItems != null)
            {
                output.SuppressOutput();
                tagHelperItems.Add(TagHelper.CreateBundleTagHelperItem());
            }
            else
            {
                await ResourceService.ProcessAsync(
                    context,
                    output,
                    new List<BundleTagHelperItem>
                    {
                        TagHelper.CreateBundleTagHelperItem()
                    },
                    TagHelper.GetNameOrNull()
                );
            }
        }
    }
}