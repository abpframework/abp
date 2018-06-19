using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
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

        protected readonly BundlingOptions Options;
        protected readonly IHybridWebRootFileProvider WebRootFileProvider;
        protected readonly IHostingEnvironment HostingEnvironment;
        protected readonly IScriptBundler ScriptBundler;
        protected readonly IStyleBundler StyleBundler;
        protected readonly IServiceProvider ServiceProvider;
        protected readonly IDynamicFileProvider DynamicFileProvider;
        protected readonly IBundleCache BundleCache;
        protected readonly IWebRequestResources RequestResources;

        public BundleManager(
            IOptions<BundlingOptions> options,
            IScriptBundler scriptBundler,
            IStyleBundler styleBundler,
            IHostingEnvironment hostingEnvironment,
            IServiceProvider serviceProvider,
            IDynamicFileProvider dynamicFileProvider,
            IBundleCache bundleCache,
            IHybridWebRootFileProvider webRootFileProvider,
            IWebRequestResources requestResources)
        {
            HostingEnvironment = hostingEnvironment;
            ScriptBundler = scriptBundler;
            ServiceProvider = serviceProvider;
            DynamicFileProvider = dynamicFileProvider;
            BundleCache = bundleCache;
            WebRootFileProvider = webRootFileProvider;
            RequestResources = requestResources;
            StyleBundler = styleBundler;
            Options = options.Value;

            Logger = NullLogger<BundleManager>.Instance;
        }

        public virtual IReadOnlyList<string> GetStyleBundleFiles(string bundleName)
        {
            return GetBundleFiles(Options.StyleBundles, bundleName, StyleBundler);
        }

        public virtual IReadOnlyList<string> GetScriptBundleFiles(string bundleName)
        {
            return GetBundleFiles(Options.ScriptBundles, bundleName, ScriptBundler);
        }

        protected virtual IReadOnlyList<string> GetBundleFiles(BundleConfigurationCollection bundles, string bundleName, IBundler bundler)
        {
            var contributors = GetContributors(bundles, bundleName);
            var files = RequestResources.Filter(CreateFileList(contributors));

            if (!IsBundlingEnabled())
            {
                return files;
            }

            var bundleRelativePath =
                Options.BundleFolderName.EnsureEndsWith('/') +
                bundleName + "." + files.JoinAsString("|").ToMd5() + "." + bundler.FileExtension;

            var cacheItem = BundleCache.GetOrAdd(bundleRelativePath, () =>
            {
                var cacheValue = new BundleCacheItem(
                    new List<string>
                    {
                        "/" + bundleRelativePath
                    }
                );

                WatchChanges(cacheValue, files, bundleRelativePath);

                var bundleResult = bundler.Bundle(
                    new BundlerContext(
                        bundleRelativePath,
                        files,
                        IsMinficationEnabled()
                    )
                );

                SaveBundleResult(bundleRelativePath, bundleResult);

                return cacheValue;
            });

            return cacheItem.Files.ToImmutableList();
        }

        private void WatchChanges(BundleCacheItem cacheValue, List<string> files, string bundleRelativePath)
        {
            lock (cacheValue.WatchDisposeHandles)
            {
                foreach (var file in files)
                {
                    var watchDisposeHandle = WebRootFileProvider.Watch(file).RegisterChangeCallback(_ =>
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
                    Encoding.UTF8.GetBytes(bundleResult.Content),
                    "/wwwroot/" + bundleRelativePath, //TODO: get rid of wwwroot!
                    fileName
                    )
                );
        }

        public virtual void CreateStyleBundle(string bundleName, Action<BundleConfiguration> configureAction)
        {
            Options.StyleBundles.TryAdd(bundleName, configureAction);
        }

        public virtual void CreateScriptBundle(string bundleName, Action<BundleConfiguration> configureAction)
        {
            Options.ScriptBundles.TryAdd(bundleName, configureAction);
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

        protected virtual List<string> CreateFileList(List<BundleContributor> contributors)
        {
            using (var scope = ServiceProvider.CreateScope())
            {
                var context = new BundleConfigurationContext(scope.ServiceProvider, scope.ServiceProvider.GetRequiredService<IVirtualFileProvider>());
                contributors.ForEach(c => c.PreConfigureBundle(context));
                contributors.ForEach(c => c.ConfigureBundle(context));
                contributors.ForEach(c => c.PostConfigureBundle(context));

                return context.Files;
            }
        }

        protected virtual List<BundleContributor> GetContributors(BundleConfigurationCollection bundles, string bundleName)
        {
            var contributors = new List<BundleContributor>();
            AddContributorsWithBaseBundles(contributors, bundles, bundleName);
            return contributors;
        }

        protected virtual void AddContributorsWithBaseBundles(List<BundleContributor> contributors, BundleConfigurationCollection bundles, string bundleName)
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