using System;
using System.Collections.Generic;

namespace Volo.Abp.AspNetCore.Mvc.UI.Bundling
{
    public static class BundleConfigurationExtensions
    {
        public static BundleConfiguration AddFiles(this BundleConfiguration bundleConfiguration, params string[] files)
        {
            bundleConfiguration.Contributors.AddFiles(files);
            return bundleConfiguration;
        }

        public static BundleConfiguration AddContributors(this BundleConfiguration bundleConfiguration, params IBundleContributor[] contributors)
        {
            if (!contributors.IsNullOrEmpty())
            {
                foreach (var contributor in contributors)
                {
                    bundleConfiguration.Contributors.Add(contributor);
                }
            }

            return bundleConfiguration;
        }

        public static BundleConfiguration AddContributors(this BundleConfiguration bundleConfiguration, params Type[] contributorTypes)
        {
            if (!contributorTypes.IsNullOrEmpty())
            {
                foreach (var contributorType in contributorTypes)
                {
                    bundleConfiguration.Contributors.Add(contributorType);
                }
            }

            return bundleConfiguration;
        }
    }
}
