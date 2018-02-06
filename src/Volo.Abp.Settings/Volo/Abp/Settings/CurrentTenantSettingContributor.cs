using System.Threading.Tasks;

namespace Volo.Abp.Settings
{
    public class CurrentTenantSettingContributor : ISettingContributor
    {
        private readonly ISettingStore _settingStore;

        public CurrentTenantSettingContributor(ISettingStore settingStore)
        {
            _settingStore = settingStore;
        }

        public async Task<string> GetOrNull(string name)
        {
            //Optimization: Get all Tenant-42 settings and cache it!
            var tenantId = 42; //Get from current tenant
            return await _settingStore.GetOrNullAsync(name, "Tenant", tenantId.ToString());
        }
    }
}