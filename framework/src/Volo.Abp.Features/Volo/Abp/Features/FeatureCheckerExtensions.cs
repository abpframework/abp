using System;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Volo.Abp.Threading;

namespace Volo.Abp.Features
{
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

        public static string GetOrNull(
            [NotNull] this IFeatureChecker featureChecker, 
            [NotNull] string name)
        {
            Check.NotNull(featureChecker, nameof(featureChecker));
            return AsyncHelper.RunSync(() => featureChecker.GetOrNullAsync(name));
        }

        public static T Get<T>(
            [NotNull] this IFeatureChecker featureChecker, 
            [NotNull] string name, 
            T defaultValue = default)
            where T : struct
        {
            return AsyncHelper.RunSync(() => featureChecker.GetAsync(name, defaultValue));
        }

        public static bool IsEnabled(
            [NotNull] this IFeatureChecker featureChecker, 
            [NotNull] string name)
        {
            return AsyncHelper.RunSync(() => featureChecker.IsEnabledAsync(name));
        }
    }
}