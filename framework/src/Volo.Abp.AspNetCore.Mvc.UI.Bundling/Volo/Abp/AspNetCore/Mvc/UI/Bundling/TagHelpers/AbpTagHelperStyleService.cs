using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Volo.Abp.AspNetCore.Security;

namespace Volo.Abp.AspNetCore.Mvc.UI.Bundling.TagHelpers;

public class AbpTagHelperStyleService : AbpTagHelperResourceService
{
    protected AbpSecurityHeadersOptions SecurityHeadersOptions;
    public AbpTagHelperStyleService(
        IBundleManager bundleManager,
        IOptions<AbpBundlingOptions> options,
        IWebHostEnvironment hostingEnvironment,
        IOptions<AbpSecurityHeadersOptions> securityHeadersOptions) : base(
            bundleManager,
            options,
            hostingEnvironment)
    {
        SecurityHeadersOptions = securityHeadersOptions.Value;
    }

    protected override void CreateBundle(string bundleName, List<BundleTagHelperItem> bundleItems)
    {
        Options.StyleBundles.TryAdd(
            bundleName,
            configuration => bundleItems.ForEach(bi => bi.AddToConfiguration(configuration)),
            HostingEnvironment.IsDevelopment() && bundleItems.Any()
        );
    }

    protected override async Task<IReadOnlyList<BundleFile>> GetBundleFilesAsync(string bundleName)
    {
        return await BundleManager.GetStyleBundleFilesAsync(bundleName);
    }

    protected override void AddHtmlTag(ViewContext viewContext, TagHelper tagHelper, TagHelperContext context, TagHelperOutput output, BundleFile file, IFileInfo? fileInfo = null)
    {
        var preload = tagHelper switch
        {
            AbpStyleTagHelper styleTagHelper => styleTagHelper.Preload,
            AbpStyleBundleTagHelper styleBundleTagHelper => styleBundleTagHelper.Preload,
            _ => false
        };

        var href = file.IsExternalFile ? file.FileName : viewContext.GetUrlHelper().Content((file.FileName + "?_v=" + fileInfo!.LastModified.UtcTicks).EnsureStartsWith('~'));
        if (preload || Options.PreloadStylesByDefault || Options.PreloadStyles.Any(x => file.FileName.StartsWith(x, StringComparison.OrdinalIgnoreCase)))
        {
            output.Content.AppendHtml(SecurityHeadersOptions.UseContentSecurityPolicyScriptNonce
                ? $"<link rel=\"preload\" href=\"{href}\" as=\"style\" abp-csp-style />{Environment.NewLine}"
                : $"<link rel=\"preload\" href=\"{href}\" as=\"style\" onload=\"this.rel='stylesheet'\" />{Environment.NewLine}");
        }
        else
        {
            output.Content.AppendHtml($"<link rel=\"stylesheet\" href=\"{href}\" />{Environment.NewLine}");
        }
    }
}
