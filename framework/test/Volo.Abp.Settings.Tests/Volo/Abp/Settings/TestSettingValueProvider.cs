using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.Settings
{
    public class TestSettingValueProvider : ISettingValueProvider, ITransientDependency
    {
        public const string ProviderName = "Test";

        private readonly Dictionary<string, string> _values;

        public string Name => ProviderName;

        public TestSettingValueProvider() 
        {
            _values = new Dictionary<string, string>();
        }

        public Task<string> GetOrNullAsync(SettingDefinition setting, string providerKey)
        {
            return Task.FromResult(_values.GetOrDefault(setting.Name));
        }

        public Task SetAsync(SettingDefinition setting, string value, string providerKey)
        {
            _values[setting.Name] = value;
            return Task.CompletedTask;
        }

        public Task ClearAsync(SettingDefinition setting, string providerKey)
        {
            _values.Remove(setting.Name);
            return Task.CompletedTask;
        }
    }
}
