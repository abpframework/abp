using System.Collections.Generic;
using Microsoft.Extensions.Options;
using Volo.DependencyInjection;

namespace Volo.Abp.AspNetCore.Mvc.Bundling
{
    public class BundleManager : IBundleManager, ITransientDependency
    {
        private readonly BundlingOptions _options;

        public BundleManager(IOptions<BundlingOptions> options)
        {
            _options = options.Value;
        }

        public List<string> GetStyleBundleFiles(string bundleName)
        {
            return _options.StyleBundles.GetFiles(bundleName);
        }

        public List<string> GetScriptBundleFiles(string bundleName)
        {
            return _options.ScriptBundles.GetFiles(bundleName);
        }
    }
}