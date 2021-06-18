using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.MultiTenancy;

namespace Volo.Abp.Settings
{
    public class TenantSettingValueProvider : SettingValueProvider
    {
        public const string ProviderName = "T";

        public override string Name => ProviderName;

        protected ICurrentTenant CurrentTenant { get; }

        public TenantSettingValueProvider(ISettingStore settingStore, ICurrentTenant currentTenant)
            : base(settingStore)
        {
            CurrentTenant = currentTenant;
        }

        public override async Task<string> GetOrNullAsync(SettingDefinition setting)
        {
            return await SettingStore.GetOrNullAsync(setting.Name, Name, CurrentTenant.Id?.ToString());
        }

        public override async Task<List<SettingValue>> GetAllAsync(SettingDefinition[] settings)
        {
            return await SettingStore.GetAllAsync(settings.Select(x => x.Name).ToArray(), Name, CurrentTenant.Id?.ToString());
        }
    }
}
