using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace Volo.Abp.AspNetCore.Mvc.UI.Bundling.TagHelpers
{
    public class AbpTagHelperStyleService : AbpTagHelperResourceService
    {
        public AbpTagHelperStyleService(
            IBundleManager bundleManager,
            IOptions<AbpBundlingOptions> options,
            IWebHostEnvironment hostingEnvironment) : base(
                bundleManager,
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

        protected override void AddHtmlTag(ViewContext viewContext, TagHelper tagHelper, TagHelperContext context, TagHelperOutput output, string file)
        {
            var preload = tagHelper switch
            {
                AbpStyleTagHelper styleTagHelper => styleTagHelper.Preload,
                AbpStyleBundleTagHelper styleBundleTagHelper => styleBundleTagHelper.Preload,
                _ => false
            };

            if (preload || Options.PreloadStylesByDefault || Options.PreloadStyles.Any(x => file.StartsWith(x, StringComparison.OrdinalIgnoreCase)))
            {
                output.Content.AppendHtml($"<link rel=\"preload\" href=\"{viewContext.GetUrlHelper().Content(file.EnsureStartsWith('~'))}\" as=\"style\" onload=\"this.rel='stylesheet'\" />{Environment.NewLine}");
            }
            else
            {
                output.Content.AppendHtml($"<link rel=\"stylesheet\" href=\"{viewContext.GetUrlHelper().Content(file.EnsureStartsWith('~'))}\" />{Environment.NewLine}");
            }
        }
    }
}
