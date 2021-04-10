using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Volo.Abp.AspNetCore.Mvc.UI.Bundling.Scripts;
using Volo.Abp.AspNetCore.Mvc.UI.Bundling.Styles;
using Volo.Abp.AspNetCore.Mvc.UI.Resources;
using Volo.Abp.AspNetCore.VirtualFileSystem;
using Volo.Abp.DependencyInjection;
using Volo.Abp.VirtualFileSystem;

namespace Volo.Abp.AspNetCore.Mvc.UI.Bundling
{
    public class BundleManager : IBundleManager, ITransientDependency
    {
        public ILogger<BundleManager> Logger { get; set; }

        protected readonly AbpBundlingOptions Options;
        protected readonly AbpBundleContributorOptions ContributorOptions;
        protected readonly IWebHostEnvironment HostingEnvironment;
        protected readonly IScriptBundler ScriptBundler;
        protected readonly IStyleBundler StyleBundler;
        protected readonly IServiceProvider ServiceProvider;
        protected readonly IDynamicFileProvider DynamicFileProvider;
        protected readonly IBundleCache BundleCache;
        protected readonly IWebRequestResources RequestResources;

        public BundleManager(
            IOptions<AbpBundlingOptions> options,
            IOptions<AbpBundleContributorOptions> contributorOptions,
            IScriptBundler scriptBundler,
            IStyleBundler styleBundler,
            IWebHostEnvironment hostingEnvironment,
            IServiceProvider serviceProvider,
            IDynamicFileProvider dynamicFileProvider,
            IBundleCache bundleCache,
            IWebRequestResources requestResources)
        {
            Options = options.Value;
            ContributorOptions = contributorOptions.Value;
            HostingEnvironment = hostingEnvironment;
            ScriptBundler = scriptBundler;
            ServiceProvider = serviceProvider;
            DynamicFileProvider = dynamicFileProvider;
            BundleCache = bundleCache;
            RequestResources = requestResources;
            StyleBundler = styleBundler;

            Logger = NullLogger<BundleManager>.Instance;
        }

        public virtual async Task<IReadOnlyList<string>> GetStyleBundleFilesAsync(string bundleName)
        {
            return await GetBundleFilesAsync(Options.StyleBundles, bundleName, StyleBundler);
        }

        public virtual async Task<IReadOnlyList<string>> GetScriptBundleFilesAsync(string bundleName)
        {
            return await GetBundleFilesAsync(Options.ScriptBundles, bundleName, ScriptBundler);
        }

        protected virtual async Task<IReadOnlyList<string>> GetBundleFilesAsync(BundleConfigurationCollection bundles, string bundleName, IBundler bundler)
        {
            var contributors = GetContributors(bundles, bundleName);
            var bundleFiles = RequestResources.TryAdd(await GetBundleFilesAsync(contributors));
            var dynamicResources = RequestResources.TryAdd(await GetDynamicResourcesAsync(contributors));

            if (!IsBundlingEnabled())
            {
                return bundleFiles.Union(dynamicResources).ToImmutableList();
            }

            var bundleRelativePath =
                Options.BundleFolderName.EnsureEndsWith('/') +
                bundleName + "." + bundleFiles.JoinAsString("|").ToMd5() + "." + bundler.FileExtension;

            var cacheItem = BundleCache.GetOrAdd(bundleRelativePath, () =>
            {
                var cacheValue = new BundleCacheItem(
                    new List<string>
                    {
                        "/" + bundleRelativePath
                    }
                );

                WatchChanges(cacheValue, bundleFiles, bundleRelativePath);

                var bundleResult = bundler.Bundle(
                    new BundlerContext(
                        bundleRelativePath,
                        bundleFiles,
                        IsMinficationEnabled()
                    )
                );

                SaveBundleResult(bundleRelativePath, bundleResult);

                return cacheValue;
            });

            return cacheItem.Files.Union(dynamicResources).ToImmutableList();
        }

        private void WatchChanges(BundleCacheItem cacheValue, List<string> files, string bundleRelativePath)
        {
            lock (cacheValue.WatchDisposeHandles)
            {
                foreach (var file in files)
                {
                    var watchDisposeHandle = HostingEnvironment.WebRootFileProvider.Watch(file).RegisterChangeCallback(_ =>
                    {
                        lock (cacheValue.WatchDisposeHandles)
                        {
                            cacheValue.WatchDisposeHandles.ForEach(h => h.Dispose());
                            cacheValue.WatchDisposeHandles.Clear();
                        }

                        BundleCache.Remove(bundleRelativePath);
                        DynamicFileProvider.Delete("/wwwroot/" + bundleRelativePath); //TODO: get rid of wwwroot!
                    }, null);

                    cacheValue.WatchDisposeHandles.Add(watchDisposeHandle);
                }
            }
        }

        protected virtual void SaveBundleResult(string bundleRelativePath, BundleResult bundleResult)
        {
            var fileName = bundleRelativePath.Substring(bundleRelativePath.IndexOf('/') + 1);

            DynamicFileProvider.AddOrUpdate(
                new InMemoryFileInfo(
                    "/wwwroot/" + bundleRelativePath, //TODO: get rid of wwwroot!
                    Encoding.UTF8.GetBytes(bundleResult.Content),
                    fileName
                )
            );
        }

        protected virtual bool IsBundlingEnabled()
        {
            switch (Options.Mode)
            {
                case BundlingMode.None:
                    return false;
                case BundlingMode.Bundle:
                case BundlingMode.BundleAndMinify:
                    return true;
                case BundlingMode.Auto:
                    return !HostingEnvironment.IsDevelopment();
                default:
                    throw new AbpException($"Unhandled {nameof(BundlingMode)}: {Options.Mode}");
            }
        }

        protected virtual bool IsMinficationEnabled()
        {
            switch (Options.Mode)
            {
                case BundlingMode.None:
                case BundlingMode.Bundle:
                    return false;
                case BundlingMode.BundleAndMinify:
                    return true;
                case BundlingMode.Auto:
                    return !HostingEnvironment.IsDevelopment();
                default:
                    throw new AbpException($"Unhandled {nameof(BundlingMode)}: {Options.Mode}");
            }
        }

        protected async Task<List<string>> GetBundleFilesAsync(List<IBundleContributor> contributors)
        {
            var context = CreateBundleConfigurationContext();

            foreach (var contributor in contributors)
            {
                await contributor.PreConfigureBundleAsync(context);
            }

            foreach (var contributor in contributors)
            {
                await contributor.ConfigureBundleAsync(context);
            }

            foreach (var contributor in contributors)
            {
                await contributor.PostConfigureBundleAsync(context);
            }

            return context.Files;
        }

        protected virtual async Task<List<string>> GetDynamicResourcesAsync(List<IBundleContributor> contributors)
        {
            var context = CreateBundleConfigurationContext();

            foreach (var contributor in contributors)
            {
                await contributor.ConfigureDynamicResourcesAsync(context);
            }

            return context.Files;
        }

        protected virtual BundleConfigurationContext CreateBundleConfigurationContext()
        {
            return new BundleConfigurationContext(ServiceProvider, HostingEnvironment.WebRootFileProvider);
        }

        protected virtual List<IBundleContributor> GetContributors(BundleConfigurationCollection bundles, string bundleName)
        {
            var contributors = new List<IBundleContributor>();

            AddContributorsWithBaseBundles(contributors, bundles, bundleName);

            for (var i = 0; i < contributors.Count; ++i)
            {
                var extensions = ContributorOptions.Extensions(contributors[i].GetType()).GetAll();
                if (extensions.Count > 0)
                {
                    contributors.InsertRange(i + 1, extensions);
                    i += extensions.Count;
                }
            }

            return contributors;
        }

        protected virtual void AddContributorsWithBaseBundles(List<IBundleContributor> contributors, BundleConfigurationCollection bundles, string bundleName)
        {
            var bundleConfiguration = bundles.Get(bundleName);

            foreach (var baseBundleName in bundleConfiguration.BaseBundles)
            {
                AddContributorsWithBaseBundles(contributors, bundles, baseBundleName); //Recursive call
            }

            var selfContributors = bundleConfiguration.Contributors.GetAll();

            if (selfContributors.Any())
            {
                contributors.AddRange(selfContributors);
            }
        }
    }
}