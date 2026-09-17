using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Volo.Abp.Authorization;

namespace Volo.Abp.Features;

public static class FeatureCheckerExtensions
{
    public static async Task<T> GetAsync<T>(
        [NotNull] this IFeatureChecker featureChecker,
        [NotNull] string name,
        T defaultValue = default)
        where T : struct
    {
        Check.NotNull(featureChecker, nameof(featureChecker));
        Check.NotNull(name, nameof(name));

        var value = await featureChecker.GetOrNullAsync(name);
        return value?.To<T>() ?? defaultValue;
    }

    public static async Task<bool> IsEnabledAsync(this IFeatureChecker featureChecker, bool requiresAll, params string[] featureNames)
    {
        if (featureNames.IsNullOrEmpty())
        {
            return true;
        }

        if (requiresAll)
        {
            foreach (var featureName in featureNames)
            {
                if (!(await featureChecker.IsEnabledAsync(featureName)))
                {
                    return false;
                }
            }

            return true;
        }

        foreach (var featureName in featureNames)
        {
            if (await featureChecker.IsEnabledAsync(featureName))
            {
                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// Checks if the specified feature is enabled and throws an <see cref="AbpAuthorizationException"/> if it is not.
    /// </summary>
    /// <param name="featureChecker">The <see cref="IFeatureChecker"/></param>
    /// <param name="featureName">The name of the feature to be checked.</param>
    public static async Task CheckEnabledAsync(this IFeatureChecker featureChecker, string featureName)
    {
        if (!(await featureChecker.IsEnabledAsync(featureName)))
        {
            throw new AbpAuthorizationException(code: AbpFeatureErrorCodes.FeatureIsNotEnabled).WithData(
                "FeatureName", featureName);
        }
    }

    /// <summary>
    /// Checks if the specified features are enabled and throws an <see cref="AbpAuthorizationException"/> if they are not.
    /// The check can either require all features to be enabled or just one, based on the <paramref name="requiresAll"/> parameter.
    /// </summary>
    /// <param name="featureChecker">The <see cref="IFeatureChecker"/></param>
    /// <param name="requiresAll">True: Requires all features to be enabled. False: Requires at least one of the features to be enabled.</param>
    /// <param name="featureNames">The names of the features to be checked.</param>
    public static async Task CheckEnabledAsync(this IFeatureChecker featureChecker, bool requiresAll, params string[] featureNames)
    {
        if (featureNames.IsNullOrEmpty())
        {
            return;
        }

        if (requiresAll)
        {
            foreach (var featureName in featureNames)
            {
                if (!(await featureChecker.IsEnabledAsync(featureName)))
                {
                    throw new AbpAuthorizationException(code: AbpFeatureErrorCodes.AllOfTheseFeaturesMustBeEnabled)
                        .WithData("FeatureNames", string.Join(", ", featureNames));
                }
            }
        }
        else
        {
            foreach (var featureName in featureNames)
            {
                if (await featureChecker.IsEnabledAsync(featureName))
                {
                    return;
                }
            }

            throw new AbpAuthorizationException(code: AbpFeatureErrorCodes.AtLeastOneOfTheseFeaturesMustBeEnabled)
                .WithData("FeatureNames", string.Join(", ", featureNames));
        }
    }
}
