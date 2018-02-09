using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.Settings
{
    public class NullSettingStore : ISettingStore, ISingletonDependency
    {
        public Task<string> GetOrNullAsync(string name, string providerName, string providerKey)
        {
            return Task.FromResult((string) null);
        }

        public Task SetAsync(string name, string value, string providerName, string providerKey)
        {
            return Task.CompletedTask;
        }

        public Task<List<SettingValue>> GetListAsync(string providerName, string providerKey)
        {
            return Task.FromResult(new List<SettingValue>());
        }

        public Task DeleteAsync(string name, string providerName, string providerKey)
        {
            return Task.CompletedTask;
        }
    }
}