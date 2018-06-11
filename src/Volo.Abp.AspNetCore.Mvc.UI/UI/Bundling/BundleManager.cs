using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Volo.Abp.AspNetCore.Mvc.UI.Bundling.Scripts;
using Volo.Abp.AspNetCore.Mvc.UI.Bundling.Styles;
using Volo.Abp.AspNetCore.Mvc.UI.Theming;
using Volo.Abp.DependencyInjection;
using Volo.Abp.IO;

namespace Volo.Abp.AspNetCore.Mvc.UI.Bundling
{
    //TODO: Do not make this singleton (since it's using scoped services), instead separate a singleton bundlecache!
    public class BundleManager : IBundleManager, ISingletonDependency
    {
        private readonly BundlingOptions _options;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IScriptBundler _scriptBundler;
        private readonly IStyleBundler _styleBundler;
        private readonly IThemeManager _themeManager;
        private readonly IServiceProvider _serviceProvider;

        private readonly ConcurrentDictionary<string, string> _cache;

        public BundleManager(
            IOptions<BundlingOptions> options,
            IScriptBundler scriptBundler,
            IStyleBundler styleBundler,
            IHostingEnvironment hostingEnvironment,
            IThemeManager themeManager,
            IServiceProvider serviceProvider)
        {
            _hostingEnvironment = hostingEnvironment;
            _scriptBundler = scriptBundler;
            _themeManager = themeManager;
            _serviceProvider = serviceProvider;
            _styleBundler = styleBundler;
            _options = options.Value;

            _cache = new ConcurrentDictionary<string, string>();
        }

        public List<string> GetStyleBundleFiles(string bundleName)
        {
            return GetBundleFiles(_options.StyleBundles, bundleName, _styleBundler);
        }

        public List<string> GetScriptBundleFiles(string bundleName)
        {
            return GetBundleFiles(_options.ScriptBundles, bundleName, _scriptBundler);
        }

        protected virtual List<string> GetBundleFiles(BundleConfigurationCollection bundles, string bundleName, IBundler bundler)
        {
            //TODO: Caching
            //TODO: Concurrency


            var files = CreateFileList(bundles, bundleName);

            if (!IsBundlingEnabled())
            {
                return files;
            }

            var bundleRelativePath = _options.BundleFolderName.EnsureEndsWith('/') + bundleName + "." + bundler.FileExtension;

            var bundleResult = bundler.Bundle(new BundlerContext(bundleRelativePath, files));

            SaveBundleResult(bundleRelativePath, bundleResult);

            return new List<string>
            {
                "/" + bundleRelativePath
            };
        }

        protected virtual void SaveBundleResult(string bundleRelativePath, BundleResult bundleResult)
        {
            var bundleFilePath = Path.Combine(_hostingEnvironment.WebRootPath, bundleRelativePath);
            DirectoryHelper.CreateIfNotExists(Path.GetDirectoryName(bundleFilePath));
            File.WriteAllText(bundleFilePath, bundleResult.Content, Encoding.UTF8);
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
            return true;
            //return !_hostingEnvironment.IsDevelopment();
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