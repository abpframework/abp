using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Volo.Abp.AspNetCore.Mvc.UI.Bundling.Scripts;
using Volo.Abp.AspNetCore.Mvc.UI.Bundling.Styles;
using Volo.Abp.AspNetCore.VirtualFileSystem;
using Volo.Abp.DependencyInjection;
using Volo.Abp.VirtualFileSystem;

namespace Volo.Abp.AspNetCore.Mvc.UI.Bundling
{
    public class BundleManager : IBundleManager, ITransientDependency
    {
        protected IHybridWebRootFileProvider WebRootFileProvider { get; }

        //TODO: Make all protected readonly to allow easily extend the bundlemanager
        private readonly BundlingOptions _options;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IScriptBundler _scriptBundler;
        private readonly IStyleBundler _styleBundler;
        private readonly IServiceProvider _serviceProvider;
        private readonly IDynamicFileProvider _dynamicFileProvider;
        private readonly IBundleCache _bundleCache;

        public BundleManager(
            IOptions<BundlingOptions> options,
            IScriptBundler scriptBundler,
            IStyleBundler styleBundler,
            IHostingEnvironment hostingEnvironment,
            IServiceProvider serviceProvider,
            IDynamicFileProvider dynamicFileProvider,
            IBundleCache bundleCache,
            IHybridWebRootFileProvider webRootFileProvider)
        {
            _hostingEnvironment = hostingEnvironment;
            _scriptBundler = scriptBundler;
            _serviceProvider = serviceProvider;
            _dynamicFileProvider = dynamicFileProvider;
            _bundleCache = bundleCache;
            WebRootFileProvider = webRootFileProvider;
            _styleBundler = styleBundler;
            _options = options.Value;
        }

        public virtual List<string> GetStyleBundleFiles(string bundleName)
        {
            return GetBundleFiles(_options.StyleBundles, bundleName, _styleBundler);
        }

        public virtual List<string> GetScriptBundleFiles(string bundleName)
        {
            return GetBundleFiles(_options.ScriptBundles, bundleName, _scriptBundler);
        }

        protected virtual List<string> GetBundleFiles(BundleConfigurationCollection bundles, string bundleName, IBundler bundler)
        {
            var bundleRelativePath = _options.BundleFolderName.EnsureEndsWith('/') + bundleName + "." + bundler.FileExtension;

            var cacheItem = _bundleCache.GetOrAdd(bundleRelativePath, () =>
            {
                var files = CreateFileList(bundles, bundleName);

                if (!IsBundlingEnabled())
                {
                    return new BundleCacheItem(files);
                }

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

            return cacheItem.Files;
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

                        _bundleCache.Remove(bundleRelativePath);
                        _dynamicFileProvider.Delete("/wwwroot/" + bundleRelativePath); //TODO: get rid of wwwroot!
                    }, null);

                    cacheValue.WatchDisposeHandles.Add(watchDisposeHandle);
                }
            }
        }

        protected virtual void SaveBundleResult(string bundleRelativePath, BundleResult bundleResult)
        {
            //TODO: Optimize?
            var fileName = bundleRelativePath.Substring(bundleRelativePath.IndexOf('/') + 1);

            _dynamicFileProvider.AddOrUpdate(
                new InMemoryFileInfo(
                    Encoding.UTF8.GetBytes(bundleResult.Content),
                    "/wwwroot/" + bundleRelativePath, //TODO: get rid of wwwroot!
                    fileName
                    )
                );

            //TODO: Saving to file option?

            //var bundleFilePath = Path.Combine(_hostingEnvironment.WebRootPath, bundleRelativePath);
            //DirectoryHelper.CreateIfNotExists(Path.GetDirectoryName(bundleFilePath));
            //File.WriteAllText(bundleFilePath, bundleResult.Content, Encoding.UTF8);
        }

        public virtual void CreateStyleBundle(string bundleName, Action<BundleConfiguration> configureAction)
        {
            _options.StyleBundles.GetOrAdd(bundleName, configureAction);
        }

        public virtual void CreateScriptBundle(string bundleName, Action<BundleConfiguration> configureAction)
        {
            _options.ScriptBundles.GetOrAdd(bundleName, configureAction);
        }

        protected virtual bool IsBundlingEnabled()
        {
            switch (_options.Mode)
            {
                case BundlingMode.None:
                    return false;
                case BundlingMode.Bundle:
                case BundlingMode.BundleAndMinify:
                    return true;
                case BundlingMode.Auto:
                    return !_hostingEnvironment.IsDevelopment();
                default:
                    throw new AbpException($"Unhandled {nameof(BundlingMode)}: {_options.Mode}");
            }
        }

        protected virtual bool IsMinficationEnabled()
        {
            switch (_options.Mode)
            {
                case BundlingMode.None:
                case BundlingMode.Bundle:
                    return false;
                case BundlingMode.BundleAndMinify:
                    return true;
                case BundlingMode.Auto:
                    return !_hostingEnvironment.IsDevelopment();
                default:
                    throw new AbpException($"Unhandled {nameof(BundlingMode)}: {_options.Mode}");
            }
        }

        protected virtual List<string> CreateFileList(BundleConfigurationCollection bundles, string bundleName)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var contributors = new List<IBundleContributor>();
                var context = new BundleConfigurationContext(scope.ServiceProvider);

                AddContributorsWithBaseBundles(contributors, bundles, context, bundleName);

                foreach (var contributor in contributors)
                {
                    contributor.ConfigureBundle(context);
                }

                return context.Files;
            }
        }

        protected virtual void AddContributorsWithBaseBundles(List<IBundleContributor> contributors, BundleConfigurationCollection bundles, BundleConfigurationContext context, string bundleName)
        {
            var bundleConfiguration = bundles.Get(bundleName);

            foreach (var baseBundleName in bundleConfiguration.BaseBundles)
            {
                AddContributorsWithBaseBundles(contributors, bundles, context, baseBundleName); //Recursive call
            }

            var selfContributors = bundleConfiguration.Contributors.GetAll();

            if (selfContributors.Any())
            {
                contributors.AddRange(selfContributors);
            }
        }
    }
}