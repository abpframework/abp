using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Volo.Abp.AspNetCore.VirtualFileSystem;

namespace Volo.Abp.AspNetCore.Mvc.UI.Bundling.TagHelpers
{
    public class AbpStyleTagHelperService : AbpBundleTagHelperServiceBase<AbpStyleTagHelper>
    {
        public AbpStyleTagHelperService(
            IBundleManager bundleManager, 
            IHybridWebRootFileProvider webRootFileProvider) 
            : base(
                bundleManager, 
                webRootFileProvider)
        {
        }

        //TODO: CreateBundle, GetBundleFiles & AddHtmlTag are identical with the AbpStyleBundleTagHelperService. Try to remove duplication!

        protected override void CreateBundle(string bundleName, List<BundleTagHelperItem> bundleItems)
        {
            BundleManager.CreateStyleBundle(
                bundleName,
                configuration => bundleItems.ForEach(bi => bi.AddToConfiguration(configuration))
            );
        }

        protected override IReadOnlyList<string> GetBundleFiles(string bundleName)
        {
            return BundleManager.GetStyleBundleFiles(bundleName);
        }

        protected override void AddHtmlTag(TagHelperContext context, TagHelperOutput output, string file)
        {
            output.Content.AppendHtml($"<link rel=\"stylesheet\" type=\"text/css\" href=\"{file}\" />{Environment.NewLine}");
        }

        protected override Task<List<BundleTagHelperItem>> GetBundleItems(TagHelperContext context, TagHelperOutput output)
        {
            return Task.FromResult(new List<BundleTagHelperItem>
            {
                TagHelper.CreateBundleTagHelperItem()
            });
        }
    }
}