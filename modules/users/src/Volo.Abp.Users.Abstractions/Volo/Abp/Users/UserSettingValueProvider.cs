using System.Threading.Tasks;
using Volo.Abp.Settings;

namespace Volo.Abp.Users
{
    //TODO: Optimization: Get all settings and cache it!
    //TODO: Think if it's true to have this provider in this project?

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
            return await SettingStore.GetOrNullAsync(setting.Name, Name, NormalizeProviderKey(providerKey));
        }

        public override Task SetAsync(SettingDefinition setting, string value, string providerKey)
        {
            return SettingStore.SetAsync(setting.Name, value, Name, NormalizeProviderKey(providerKey));
        }

        public override Task ClearAsync(SettingDefinition setting, string providerKey)
        {
            return SettingStore.DeleteAsync(setting.Name, Name, NormalizeProviderKey(providerKey));
        }

        private string NormalizeProviderKey(string providerKey)
        {
            if (providerKey == null && CurrentUser.Id.HasValue)
            {
                return CurrentUser.Id.Value.ToString();
            }

            return providerKey;
        }
    }
}