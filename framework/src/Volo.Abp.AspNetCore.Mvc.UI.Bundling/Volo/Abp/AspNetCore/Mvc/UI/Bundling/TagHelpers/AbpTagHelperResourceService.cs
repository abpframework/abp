using JetBrains.Annotations;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Volo.Abp.AspNetCore.VirtualFileSystem;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.AspNetCore.Mvc.UI.Bundling.TagHelpers
{
    public abstract class AbpTagHelperResourceService : ITransientDependency
    {
        public ILogger<AbpTagHelperResourceService> Logger { get; set; }

        protected IHttpContextAccessor HttpContextAccessor { get; }
        protected IBundleManager BundleManager { get; }
        protected IWebContentFileProvider WebContentFileProvider { get; }
        protected IWebHostEnvironment HostingEnvironment { get; }
        protected readonly AbpBundlingOptions Options;

        protected AbpTagHelperResourceService(
            IBundleManager bundleManager,
            IWebContentFileProvider webContentFileProvider,
            IOptions<AbpBundlingOptions> options,
            IWebHostEnvironment hostingEnvironment,
            IHttpContextAccessor httpContextAccessor)
        {
            BundleManager = bundleManager;
            WebContentFileProvider = webContentFileProvider;
            HostingEnvironment = hostingEnvironment;
            HttpContextAccessor = httpContextAccessor;
            Options = options.Value;

            Logger = NullLogger<AbpTagHelperResourceService>.Instance;
        }

        public virtual async Task ProcessAsync(
            [NotNull] TagHelperContext context,
            [NotNull] TagHelperOutput output,
            [NotNull] List<BundleTagHelperItem> bundleItems,
            [CanBeNull] string bundleName = null)
        {
            Check.NotNull(context, nameof(context));
            Check.NotNull(output, nameof(output));
            Check.NotNull(bundleItems, nameof(bundleItems));

            var stopwatch = Stopwatch.StartNew();

            output.TagName = null;

            if (bundleName.IsNullOrEmpty())
            {
                bundleName = GenerateBundleName(bundleItems);
            }

            CreateBundle(bundleName, bundleItems);

            var bundleFiles = await GetBundleFilesAsync(bundleName);

            output.Content.Clear();

            foreach (var bundleFile in bundleFiles)
            {
                var file = WebContentFileProvider.GetFileInfo(bundleFile);

                 if (file == null || !file.Exists)
                {
                    throw new AbpException($"Could not find the bundle file '{bundleFile}' from {nameof(IWebContentFileProvider)}");
                }

                AddHtmlTag(context, output, ResolveUrl(bundleFile) + "?_v=" + file.LastModified.UtcTicks);
            }

            stopwatch.Stop();
            Logger.LogDebug($"Added bundle '{bundleName}' to the page in {stopwatch.Elapsed.TotalMilliseconds:0.00} ms.");
        }

        protected abstract void CreateBundle(string bundleName, List<BundleTagHelperItem> bundleItems);

        protected abstract Task<IReadOnlyList<string>> GetBundleFilesAsync(string bundleName);

        protected abstract void AddHtmlTag(TagHelperContext context, TagHelperOutput output, string file);

        protected virtual string GenerateBundleName(List<BundleTagHelperItem> bundleItems)
        {
            return bundleItems.JoinAsString("|").ToMd5();
        }

        protected virtual string ResolveUrl(string contentPath)
        {
            if (contentPath[0] != '~' && !Options.IsBundlingEnabled(HostingEnvironment))
            {
                return contentPath;
            }

            var segment = new PathString(contentPath.TrimStart('~'));
            var applicationPath = HttpContextAccessor.HttpContext.Request.PathBase;

            return applicationPath.Add(segment).Value;

        }
    }
}
