using JetBrains.Annotations;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
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

        protected IBundleManager BundleManager { get; }
        protected IHybridWebRootFileProvider WebRootFileProvider { get; }
        protected IHostingEnvironment HostingEnvironment { get; }
        protected readonly BundlingOptions Options;
        
        protected AbpTagHelperResourceService(
            IBundleManager bundleManager,
            IHybridWebRootFileProvider webRootFileProvider,
            IOptions<BundlingOptions> options, 
            IHostingEnvironment hostingEnvironment)
        {
            BundleManager = bundleManager;
            WebRootFileProvider = webRootFileProvider;
            HostingEnvironment = hostingEnvironment;
            Options = options.Value;

            Logger = NullLogger<AbpTagHelperResourceService>.Instance;
        }

        public virtual Task ProcessAsync(
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

            var bundleFiles = GetBundleFiles(bundleName);

            output.Content.Clear();

            foreach (var bundleFile in bundleFiles)
            {
                var file = WebRootFileProvider.GetFileInfo(bundleFile);
                if (file == null)
                {
                    throw new AbpException($"Could not find the bundle file from {nameof(IHybridWebRootFileProvider)}");
                }

                AddHtmlTag(context, output, bundleFile + "?_v=" + file.LastModified.UtcTicks);
            }

            stopwatch.Stop();
            Logger.LogDebug($"Added bundle '{bundleName}' to the page in {stopwatch.Elapsed.TotalMilliseconds:0.00} ms.");

            return Task.CompletedTask;
        }

        protected abstract void CreateBundle(string bundleName, List<BundleTagHelperItem> bundleItems);

        protected abstract IReadOnlyList<string> GetBundleFiles(string bundleName);

        protected abstract void AddHtmlTag(TagHelperContext context, TagHelperOutput output, string file);

        protected virtual string GenerateBundleName(List<BundleTagHelperItem> bundleItems)
        {
            return bundleItems.JoinAsString("|").ToMd5();
        }
    }
}