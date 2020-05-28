using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Volo.Abp.AspNetCore.VirtualFileSystem;

namespace Volo.Abp.AspNetCore.Mvc.UI.Bundling.TagHelpers
{
    public class AbpTagHelperScriptService : AbpTagHelperResourceService
    {
        public AbpTagHelperScriptService(
            IBundleManager bundleManager,
            IWebContentFileProvider webContentFileProvider,
            IOptions<AbpBundlingOptions> options,
            IWebHostEnvironment hostingEnvironment,
            IUrlHelperFactory urlHelperFactory
            ) : base(
                bundleManager,
                webContentFileProvider,
                options,
                hostingEnvironment,
                urlHelperFactory)
        {
        }

        protected override void CreateBundle(string bundleName, List<BundleTagHelperItem> bundleItems)
        {
            Options.ScriptBundles.TryAdd(
                bundleName,
                configuration => bundleItems.ForEach(bi => bi.AddToConfiguration(configuration)),
                HostingEnvironment.IsDevelopment() && bundleItems.Any()
            );
        }

        protected override async Task<IReadOnlyList<string>> GetBundleFilesAsync(string bundleName)
        {
            return await BundleManager.GetScriptBundleFilesAsync(bundleName);
        }

        protected override void AddHtmlTag(TagHelperContext context, TagHelperOutput output, string file)
        {
            output.Content.AppendHtml($"<script src=\"{ResolveUrl(file)}\"></script>{Environment.NewLine}");
        }
    }
}
