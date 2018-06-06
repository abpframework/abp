using System.Collections.Generic;

namespace Volo.Abp.AspNetCore.Mvc.UI.Bundling
{
    public class BundleCollection
    {
        private readonly Dictionary<string, BundleConfiguration> _bundleContributors;

        public BundleCollection()
        {
            _bundleContributors = new Dictionary<string, BundleConfiguration>();
        }

        //TODO: Seperate to Add and WithFiles/WithContributors methods instead of coupling
        public BundleConfiguration Add(string bundleName)
        {
            if (_bundleContributors.ContainsKey(bundleName))
            {
                throw new AbpException($"There is already a bundle added with given {nameof(bundleName)}: {bundleName}");
            }

            var bundleConfiguration = new BundleConfiguration(bundleName);
            _bundleContributors.Add(bundleName, bundleConfiguration);
            return bundleConfiguration;
        }

        public BundleConfiguration Get(string bundleName)
        {
            if (!_bundleContributors.ContainsKey(bundleName))
            {
                throw new AbpException($"There is no bundle added with given {nameof(bundleName)}: {bundleName}");
            }

            return _bundleContributors[bundleName];
        }

        public BundleConfiguration GetOrAdd(string bundleName)
        {
            return _bundleContributors.GetOrAdd(bundleName, () => new BundleConfiguration(bundleName));
        }

        public List<string> GetFiles(string bundleName)
        {
            var bundleConfiguration = _bundleContributors.GetOrDefault(bundleName);
            if (bundleConfiguration == null)
            {
                throw new AbpException("Undefined bundle: " + bundleName);
            }

            var files = new List<string>();

            foreach (var contributor in bundleConfiguration.Contributors)
            {
                contributor.Contribute(files);
            }

            return files;
        }
    }
}