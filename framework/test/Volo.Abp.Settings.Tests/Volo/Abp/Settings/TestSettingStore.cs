using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.Settings
{
    [Dependency(ReplaceServices = true)]
    public class TestSettingStore : ISettingStore, ISingletonDependency
    {
        private readonly Dictionary<string, string> _values;

        public TestSettingStore()
        {
            _values = new Dictionary<string, string>();
        }

        public Task<string> GetOrNullAsync(string name, string providerName, string providerKey)
        {
            return Task.FromResult(_values.GetOrDefault(name));
        }

        public Task SetAsync(string name, string value, string providerName, string providerKey)
        {
            _values[name] = value;
            return Task.CompletedTask;
        }

        public Task<List<SettingValue>> GetListAsync(string providerName, string providerKey)
        {
            return Task.FromResult(new List<SettingValue>());
        }

        public Task DeleteAsync(string name, string providerName, string providerKey)
        {
            _values.Remove(name);
            return Task.CompletedTask;
        }
    }
}
