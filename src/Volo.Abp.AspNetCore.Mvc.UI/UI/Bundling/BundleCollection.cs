using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Volo.Abp.AspNetCore.Mvc.UI.Bundling
{
    public class BundleCollection
    {
        private readonly ConcurrentDictionary<string, BundleConfiguration> _bundleContributors;

        public BundleCollection()
        {
            _bundleContributors = new ConcurrentDictionary<string, BundleConfiguration>();
        }

        public BundleConfiguration Add(string bundleName)
        {
            if (_bundleContributors.ContainsKey(bundleName))
            {
                throw new AbpException($"There is already a bundle added with given {nameof(bundleName)}: {bundleName}");
            }

            return _bundleContributors.AddOrUpdate(bundleName, new BundleConfiguration(bundleName), (n, c) => c);
        }

        public BundleConfiguration Get(string bundleName)
        {
            CheckBundle(bundleName);

            return _bundleContributors[bundleName];
        }

        public BundleConfiguration GetOrAdd(string bundleName)
        {
            return GetOrAdd(bundleName, c => { });
        }

        internal BundleConfiguration GetOrAdd(string bundleName, Action<BundleConfiguration> configureAction)
        {
            return _bundleContributors.GetOrAdd(
                bundleName,
                () =>
                {
                    var configuration = new BundleConfiguration(bundleName);
                    configureAction.Invoke(configuration);
                    return configuration;
                });
        }

        public List<string> GetFiles(string bundleName)
        {
            CheckBundle(bundleName);

            var files = new List<string>();

            var bundleConfiguration = _bundleContributors[bundleName];
            foreach (var contributor in bundleConfiguration.Contributors)
            {
                contributor.Contribute(files);
            }

            return files;
        }

        private void CheckBundle(string bundleName)
        {
            if (!_bundleContributors.ContainsKey(bundleName))
            {
                throw new AbpException($"There is no bundle with given {nameof(bundleName)}: {bundleName}");
            }
        }
    }
}