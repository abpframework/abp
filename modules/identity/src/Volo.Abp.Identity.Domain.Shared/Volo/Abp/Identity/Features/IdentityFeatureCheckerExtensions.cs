using System;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Volo.Abp.Features;

namespace Volo.Abp.Identity.Features
{
    public static class IdentityFeatureCheckerExtensions
    {
        public static async Task<IdentityTwoFactorBehaviour> GetIdentityTwoFactorBehaviour([NotNull] this IFeatureChecker featureChecker)
        {
            Check.NotNull(featureChecker, nameof(featureChecker));

            var value = await featureChecker.GetOrNullAsync(IdentityFeature.TwoFactor);
            if (value.IsNullOrWhiteSpace() || !Enum.TryParse<IdentityTwoFactorBehaviour>(value, out var behaviour))
            {
                throw new AbpException($"{IdentityFeature.TwoFactor} feature value is invalid");
            }

            return behaviour;
        }
    }
}
