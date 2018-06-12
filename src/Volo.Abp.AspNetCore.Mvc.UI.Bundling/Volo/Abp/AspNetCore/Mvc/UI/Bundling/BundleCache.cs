using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.AspNetCore.Mvc.UI.Bundling
{
    public class BundleCache : IBundleCache, ISingletonDependency
    {
        private readonly ConcurrentDictionary<string, List<string>> _cache;

        public BundleCache()
        {
            _cache = new ConcurrentDictionary<string, List<string>>();
        }

        public List<string> GetFiles(string bundleName, Func<List<string>> factory)
        {
            return _cache.GetOrAdd(bundleName, factory);
        }
    }
}