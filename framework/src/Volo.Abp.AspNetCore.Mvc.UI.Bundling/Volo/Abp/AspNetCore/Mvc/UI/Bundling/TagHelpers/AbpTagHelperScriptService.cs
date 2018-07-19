using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.Options;
using Volo.Abp.AspNetCore.VirtualFileSystem;

namespace Volo.Abp.AspNetCore.Mvc.UI.Bundling.TagHelpers
{
    public class AbpTagHelperScriptService : AbpTagHelperResourceService
    {
        public AbpTagHelperScriptService(
            IBundleManager bundleManager,
            IHybridWebRootFileProvider webRootFileProvider,
            IOptions<BundlingOptions> options
            ) : base(
                bundleManager,
                webRootFileProvider,
                options)
        {
        }

        protected override void CreateBundle(string bundleName, List<BundleTagHelperItem> bundleItems)
        {
            Options.StyleBundles.TryAdd(
                bundleName,
                configuration => bundleItems.ForEach(bi => bi.AddToConfiguration(configuration))
            );
        }

        protected override IReadOnlyList<string> GetBundleFiles(string bundleName)
        {
            return BundleManager.GetScriptBundleFiles(bundleName);
        }

        protected override void AddHtmlTag(TagHelperContext context, TagHelperOutput output, string file)
        {
            output.Content.AppendHtml($"<script src=\"{file}\" type=\"text/javascript\"></script>{Environment.NewLine}");
        }
    }
}