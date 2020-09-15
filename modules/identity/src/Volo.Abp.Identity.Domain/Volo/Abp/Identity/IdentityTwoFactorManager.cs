using System.Threading.Tasks;
using Volo.Abp.Domain.Services;
using Volo.Abp.Features;
using Volo.Abp.Identity.Features;
using Volo.Abp.Settings;

namespace Volo.Abp.Identity
{
    public class IdentityTwoFactorManager : IDomainService
    {
        protected IFeatureChecker FeatureChecker { get; }

        protected ISettingProvider SettingProvider { get; }

        public IdentityTwoFactorManager(IFeatureChecker featureChecker, ISettingProvider settingProvider)
        {
            FeatureChecker = featureChecker;
            SettingProvider = settingProvider;
        }

        public virtual async Task<bool> IsOptionalAsync()
        {
            var feature = await FeatureChecker.GetIdentityTwoFactorBehaviour();
            if (feature == IdentityTwoFactorBehaviour.Optional)
            {
                var setting = await SettingProvider.GetIdentityTwoFactorBehaviour();
                if (setting == IdentityTwoFactorBehaviour.Optional)
                {
                    return true;
                }
            }

            return false;
        }

        public virtual async Task<bool> IsForcedEnableAsync()
        {
            var feature = await FeatureChecker.GetIdentityTwoFactorBehaviour();
            if (feature == IdentityTwoFactorBehaviour.Forced)
            {
                return true;
            }

            var setting = await SettingProvider.GetIdentityTwoFactorBehaviour();
            if (setting == IdentityTwoFactorBehaviour.Forced)
            {
                return true;
            }

            return false;
        }

        public virtual async Task<bool> IsForcedDisableAsync()
        {
            var feature = await FeatureChecker.GetIdentityTwoFactorBehaviour();
            if (feature == IdentityTwoFactorBehaviour.Disabled)
            {
                return true;
            }

            var setting = await SettingProvider.GetIdentityTwoFactorBehaviour();
            if (setting == IdentityTwoFactorBehaviour.Disabled)
            {
                return true;
            }
            return false;
        }
    }
}
