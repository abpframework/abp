using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Settings;

namespace Volo.Abp.SettingManagement
{
    public class SettingStore : ISettingStore, ITransientDependency
    {
        protected ISettingManagementStore ManagementStore { get; }

        public SettingStore(ISettingManagementStore managementStore)
        {
            ManagementStore = managementStore;
        }

        public virtual Task<string> GetOrNullAsync(string name, string providerName, string providerKey)
        {
            return ManagementStore.GetOrNullAsync(name, providerName, providerKey);
        }
    }
}
