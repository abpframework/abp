using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Volo.Abp.AspNetCore.Mvc.UI.Theming;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.AspNetCore.Mvc.UI.Bundling
{
    //TODO: Do not make this singleton (since it's using scoped services), instead separate a singleton bundlecache!
    public class BundleManager : IBundleManager, ISingletonDependency
    {
        private readonly BundlingOptions _options;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IBundler _bundler;
        private readonly IThemeManager _themeManager;
        private readonly IServiceProvider _serviceProvider;

        private readonly ConcurrentDictionary<string, string> _cache;

        public BundleManager(
            IOptions<BundlingOptions> options,
            IHostingEnvironment hostingEnvironment,
            IBundler bundler,
            IThemeManager themeManager,
            IServiceProvider serviceProvider)
        {
            _hostingEnvironment = hostingEnvironment;
            _bundler = bundler;
            _themeManager = themeManager;
            _serviceProvider = serviceProvider;
            _options = options.Value;

            _cache = new ConcurrentDictionary<string, string>();
        }

        public List<string> GetStyleBundleFiles(string bundleName)
        {
            return GetFiles(_options.StyleBundles.Get(bundleName));
        }

        public List<string> GetScriptBundleFiles(string bundleName)
        {
            return GetFiles(_options.ScriptBundles.Get(bundleName));
        }

        public void CreateStyleBundle(string bundleName, Action<BundleConfiguration> configureAction)
        {
            _options.StyleBundles.GetOrAdd(bundleName, configureAction);
        }

        public void CreateScriptBundle(string bundleName, Action<BundleConfiguration> configureAction)
        {
            _options.ScriptBundles.GetOrAdd(bundleName, configureAction);
        }

        protected virtual List<string> GetFiles(BundleConfiguration bundleConfiguration)
        {
            //TODO: Caching, Bundling & Minifying!

            using (var scope = _serviceProvider.CreateScope())
            {

                var context = new BundleConfigurationContext(
                    new List<string>(),
                    _themeManager.CurrentTheme,
                    scope.ServiceProvider
                );

                foreach (var contributor in bundleConfiguration.Contributors.GetAll())
                {
                    contributor.ConfigureBundle(context);
                }

                return context.Files;
            }
        }
    }
}