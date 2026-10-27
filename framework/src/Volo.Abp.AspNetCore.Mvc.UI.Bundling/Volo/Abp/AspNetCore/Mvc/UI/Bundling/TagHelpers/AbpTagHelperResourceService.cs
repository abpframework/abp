using JetBrains.Annotations;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Volo.Abp.AspNetCore.Bundling;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.AspNetCore.Mvc.UI.Bundling.TagHelpers;

public abstract class AbpTagHelperResourceService : ITransientDependency
{
    public ILogger<AbpTagHelperResourceService> Logger { get; set; }
    protected IBundleManager BundleManager { get; }
    protected IWebHostEnvironment HostingEnvironment { get; }
    protected AbpBundlingOptions Options { get; }

    protected AbpTagHelperResourceService(
        IBundleManager bundleManager,
        IOptions<AbpBundlingOptions> options,
        IWebHostEnvironment hostingEnvironment)
    {
        BundleManager = bundleManager;
        HostingEnvironment = hostingEnvironment;
        Options = options.Value;

        Logger = NullLogger<AbpTagHelperResourceService>.Instance;
    }

    public virtual async Task ProcessAsync(
        [NotNull] ViewContext viewContext,
        [NotNull] TagHelper tagHelper,
        [NotNull] TagHelperContext context,
        [NotNull] TagHelperOutput output,
        [NotNull] List<BundleTagHelperItem> bundleItems,
        string? bundleName = null)
    {
        Check.NotNull(viewContext, nameof(viewContext));
        Check.NotNull(context, nameof(context));
        Check.NotNull(output, nameof(output));
        Check.NotNull(bundleItems, nameof(bundleItems));

        var stopwatch = Stopwatch.StartNew();

        output.TagName = null;

        if (bundleName.IsNullOrEmpty())
        {
            bundleName = GenerateBundleName(bundleItems);
        }

        CreateBundle(bundleName!, bundleItems);

        var bundleFiles = await GetBundleFilesAsync(bundleName!);

        output.Content.Clear();

        foreach (var bundleFile in bundleFiles)
        {
            if (bundleFile.IsExternalFile)
            {
                AddHtmlTag(viewContext, tagHelper, context, output, bundleFile, null);
            }
            else
            {
                var file = HostingEnvironment.WebRootFileProvider.GetFileInfo(bundleFile.FileName);
                if (file == null || !file.Exists)
                {
                    Logger.LogError($"Could not find the bundle file '{bundleFile.FileName}' for the bundle '{bundleName}'!");
                    AddErrorScript(viewContext, tagHelper, context, output, bundleFile, bundleName!);
                    continue;
                }

                if (file.Length > 0)
                {
                    AddHtmlTag(viewContext, tagHelper, context, output, bundleFile, file);
                }
            }
        }

        stopwatch.Stop();
        Logger.LogDebug($"Added bundle '{bundleName}' to the page in {stopwatch.Elapsed.TotalMilliseconds:0.00} ms.");
    }

    protected abstract void CreateBundle(string bundleName, List<BundleTagHelperItem> bundleItems);

    protected abstract Task<IReadOnlyList<BundleFile>> GetBundleFilesAsync(string bundleName);

    protected abstract void AddHtmlTag(ViewContext viewContext, TagHelper tagHelper, TagHelperContext context, TagHelperOutput output, BundleFile file, IFileInfo? fileInfo = null);

    protected virtual void AddErrorScript(ViewContext viewContext, TagHelper tagHelper, TagHelperContext context, TagHelperOutput output, BundleFile file, string bundleName)
    {
        output.Content.AppendHtml($"<script>console.log(\"%cCould not find the bundle file '{file.FileName}' for the bundle '{bundleName}'!\", 'background: yellow; color: black; font-size:20px;');</script>{Environment.NewLine}");
    }

    /// <summary>
    /// External files are not bundled, but an external file given as a local path ("/x" or "~/x")
    /// is still served by this application, so it needs the request's PathBase like the other files.
    /// A "/x" path that already starts with the PathBase is kept as is, so URLs that were prefixed manually keep working.
    /// </summary>
    protected virtual string GetExternalFileUrl(ViewContext viewContext, BundleFile file)
    {
        var fileName = file.FileName;
        if (fileName.StartsWith("~/", StringComparison.Ordinal))
        {
            return viewContext.GetUrlHelper().Content(fileName);
        }

        if (fileName.StartsWith("/", StringComparison.Ordinal) && !fileName.StartsWith("//", StringComparison.Ordinal))
        {
            var pathBase = viewContext.HttpContext.Request.PathBase;
            var queryIndex = fileName.IndexOfAny(['?', '#']);
            var path = new PathString(queryIndex < 0 ? fileName : fileName.Substring(0, queryIndex));
            if (pathBase.HasValue && path.StartsWithSegments(pathBase))
            {
                return fileName;
            }

            return viewContext.GetUrlHelper().Content("~" + fileName);
        }

        return fileName;
    }

    protected virtual string GenerateBundleName(List<BundleTagHelperItem> bundleItems)
    {
        return bundleItems.JoinAsString("|").ToMd5();
    }
}
