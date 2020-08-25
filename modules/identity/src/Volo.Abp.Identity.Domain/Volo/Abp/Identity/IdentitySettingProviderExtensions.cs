using System;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Volo.Abp.Identity.Features;
using Volo.Abp.Identity.Settings;
using Volo.Abp.Settings;

namespace Volo.Abp.Identity
{
    public static class IdentitySettingProviderExtensions
    {
        public static async Task<IdentityTwoFactorBehaviour> GetIdentityTwoFactorBehaviour([NotNull] this ISettingProvider settingProvider)
        {
            Check.NotNull(settingProvider, nameof(settingProvider));

            var value = await settingProvider.GetOrNullAsync(IdentitySettingNames.TwoFactor.Behaviour);
            if (value.IsNullOrWhiteSpace() || !Enum.TryParse<IdentityTwoFactorBehaviour>(value, out var behaviour))
            {
                throw new AbpException($"{IdentitySettingNames.TwoFactor.Behaviour} setting value is invalid");
            }

            return behaviour;
        }
    }
}
