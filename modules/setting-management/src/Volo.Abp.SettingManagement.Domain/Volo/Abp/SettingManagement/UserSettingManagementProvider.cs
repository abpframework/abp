using Volo.Abp.DependencyInjection;
using Volo.Abp.Settings;
using Volo.Abp.Users;

namespace Volo.Abp.SettingManagement
{
    public class UserSettingManagementProvider : SettingManagementProvider, ITransientDependency
    {
        public override string Name => UserSettingValueProvider.ProviderName;

        protected ICurrentUser CurrentUser { get; }

        public UserSettingManagementProvider(
            ISettingManagementStore settingManagementStore,
            ICurrentUser currentUser)
            : base(settingManagementStore)
        {
            CurrentUser = currentUser;
        }

        protected override string NormalizeProviderKey(string providerKey)
        {
            if (providerKey != null)
            {
                return providerKey;
            }

            return CurrentUser.Id?.ToString();
        }
    }
}