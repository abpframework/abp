using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.Settings
{
    public class NullSettingStore : ISettingStore, ISingletonDependency
    {
        public ILogger<NullSettingStore> Logger { get; set; }

        public NullSettingStore()
        {
            Logger = NullLogger<NullSettingStore>.Instance;
        }

        public Task<string> GetOrNullAsync(string name, string providerName, string providerKey)
        {
            return Task.FromResult((string) null);
        }

        public Task SetAsync(string name, string value, string providerName, string providerKey)
        {
            Logger.LogWarning($"Setting the value for {name} is not possible because current setting store is {nameof(NullSettingStore)}");
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