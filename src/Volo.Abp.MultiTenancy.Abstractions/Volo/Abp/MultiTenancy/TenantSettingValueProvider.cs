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
            if (providerKey == null)
            {
                if (CurrentTenant.Id == null)
                {
                    return null;
                }

                providerKey = CurrentTenant.Id.ToString();
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