using System;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Volo.Abp.Identity.Features;
using Volo.Abp.Settings;

namespace Volo.Abp.Identity.Settings
{
    public static class IdentityTwoFactorBehaviourSettingHelper
    {
        public static async Task<IdentityTwoFactorBehaviour> Get([NotNull] ISettingProvider settingProvider)
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
