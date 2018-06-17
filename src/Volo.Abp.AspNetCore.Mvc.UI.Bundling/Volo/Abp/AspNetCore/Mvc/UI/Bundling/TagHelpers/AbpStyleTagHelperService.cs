using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers;

namespace Volo.Abp.AspNetCore.Mvc.UI.Bundling.TagHelpers
{
    public class AbpStyleTagHelperService : AbpTagHelperService<AbpStyleTagHelper>
    {
        protected AbpTagHelperStyleHelper ResourceHelper { get; }

        public AbpStyleTagHelperService(AbpTagHelperStyleHelper resourceHelper)
        {
            ResourceHelper = resourceHelper;
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
                await ResourceHelper.ProcessAsync(
                    context,
                    output,
                    TagHelper.GetNameOrNull(),
                    new List<BundleTagHelperItem>
                    {
                        TagHelper.CreateBundleTagHelperItem()
                    }
                );
            }
        }
    }
}