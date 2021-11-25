using System;

namespace Volo.Abp.AspNetCore.Mvc.UI.Bundling;

public static class BundleConfigurationExtensions
{
    public static BundleConfiguration AddFiles(this BundleConfiguration bundleConfiguration, params string[] files)
    {
        bundleConfiguration.Contributors.AddFiles(files);
        return bundleConfiguration;
    }

    public static BundleConfiguration AddContributors(this BundleConfiguration bundleConfiguration, params IBundleContributor[] contributors)
    {
        Check.NotNull(contributors, nameof(contributors));

        foreach (var contributor in contributors)
        {
            bundleConfiguration.Contributors.Add(contributor);
        }

        return bundleConfiguration;
    }

    public static BundleConfiguration AddBaseBundles(this BundleConfiguration bundleConfiguration, params string[] bundleNames)
    {
        Check.NotNull(bundleNames, nameof(bundleNames));

        bundleConfiguration.BaseBundles.AddRange(bundleNames);

        return bundleConfiguration;
    }

    public static BundleConfiguration AddContributors(this BundleConfiguration bundleConfiguration, params Type[] contributorTypes)
    {
        Check.NotNull(contributorTypes, nameof(contributorTypes));

        foreach (var contributorType in contributorTypes)
        {
            bundleConfiguration.Contributors.Add(contributorType);
        }

        return bundleConfiguration;
    }
}
