using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Volo.Abp.AspNetCore.VirtualFileSystem;

namespace Volo.Abp.AspNetCore.Mvc.UI.Bundling.TagHelpers
{
    public class AbpTagHelperStyleService : AbpTagHelperResourceService
    {
        public AbpTagHelperStyleService(
            IBundleManager bundleManager,
            IWebContentFileProvider webContentFileProvider,
            IOptions<AbpBundlingOptions> options,
            IWebHostEnvironment hostingEnvironment
            ) : base(
                bundleManager,
                webContentFileProvider,
                options,
                hostingEnvironment)
        {
        }

        protected override void CreateBundle(string bundleName, List<BundleTagHelperItem> bundleItems)
        {
            Options.StyleBundles.TryAdd(
                bundleName,
                configuration => bundleItems.ForEach(bi => bi.AddToConfiguration(configuration)),
                HostingEnvironment.IsDevelopment() && bundleItems.Any()
            );
        }

        protected override async Task<IReadOnlyList<string>> GetBundleFilesAsync(string bundleName)
        {
            return await BundleManager.GetStyleBundleFilesAsync(bundleName);
        }

        protected override void AddHtmlTag(ViewContext viewContext, TagHelperContext context, TagHelperOutput output, string file)
        {
            output.Content.AppendHtml($"<link rel=\"stylesheet\" href=\"{viewContext.GetUrlHelper().Content(file.EnsureStartsWith('~'))}\" />{Environment.NewLine}");
        }
    }
}
