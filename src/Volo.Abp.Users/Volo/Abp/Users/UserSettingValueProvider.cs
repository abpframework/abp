using System.Threading.Tasks;
using Volo.Abp.Settings;

namespace Volo.Abp.Users
{
    //TODO: Optimization: Get all settings and cache it!

    public class UserSettingValueProvider : SettingValueProvider
    {
        public const string ProviderName = "User";

        public override string Name => ProviderName;

        protected ICurrentUser CurrentUser { get; }

        public UserSettingValueProvider(ISettingStore settingStore, ICurrentUser currentUser)
            : base(settingStore)
        {
            CurrentUser = currentUser;
        }

        public override async Task<string> GetOrNullAsync(SettingDefinition setting, string providerKey)
        {
            if (providerKey == null)
            {
                if (CurrentUser.Id == null)
                {
                    return null;
                }

                providerKey = CurrentUser.Id.ToString();
            }

            return await SettingStore.GetOrNullAsync(setting.Name, Name, providerKey);
        }

        public override Task SetAsync(SettingDefinition setting, string value, string providerKey)
        {
            return SettingStore.SetAsync(setting.Name, value, Name, providerKey);
        }

        public override Task ClearAsync(SettingDefinition setting, string providerKey)
        {
            return SettingStore.DeleteAsync(setting.Name, Name, providerKey);
        }
    }
}