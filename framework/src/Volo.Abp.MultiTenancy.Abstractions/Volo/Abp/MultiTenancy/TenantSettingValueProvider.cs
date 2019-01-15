using System.Threading.Tasks;
using Volo.Abp.Settings;

namespace Volo.Abp.MultiTenancy
{
    public class TenantSettingValueProvider : SettingValueProvider
    {
        public const string ProviderName = "Tenant";

        public override string Name => ProviderName;

        protected ICurrentTenant CurrentTenant { get; }

        public TenantSettingValueProvider(ISettingStore settingStore, ICurrentTenant currentTenant)
            : base(settingStore)
        {
            CurrentTenant = currentTenant;
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
            if (providerKey == null && CurrentTenant.Id.HasValue)
            {
                return CurrentTenant.Id.Value.ToString();
            }

            return providerKey;
        }
    }
}