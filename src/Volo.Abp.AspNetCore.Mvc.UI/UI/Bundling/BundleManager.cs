using System.Collections.Concurrent;
using System.Collections.Generic;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Options;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.AspNetCore.Mvc.UI.Bundling
{
    public class BundleManager : IBundleManager, ISingletonDependency
    {
        private readonly BundlingOptions _options;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IBundler _bundler;

        private readonly ConcurrentDictionary<string, string> _cache;

        public BundleManager(
            IOptions<BundlingOptions> options,
            IHostingEnvironment hostingEnvironment,
            IBundler bundler)
        {
            _hostingEnvironment = hostingEnvironment;
            _bundler = bundler;
            _options = options.Value;

            _cache = new ConcurrentDictionary<string, string>();
        }

        public List<string> GetStyleBundleFiles(string bundleName)
        {
            return _options.StyleBundles.GetFiles(bundleName);
        }

        public List<string> GetScriptBundleFiles(string bundleName)
        {
            //if (_hostingEnvironment.IsDevelopment())
            {
                return _options.ScriptBundles.GetFiles(bundleName);
            }

            return new List<string>
            {
                _cache.GetOrAdd(
                    "SCRIPT:" + bundleName,
                    () => _bundler.CreateBundle(
                        _options.ScriptBundles.GetFiles(bundleName)
                    )
                )
            };
        }
    }
}