using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.Settings
{
    public class ConfigurationSettingValueProvider : ISettingValueProvider, ITransientDependency
    {
        public const string ConfigurationNamePrefix = "Settings:";

        public const string ProviderName = "C";

        public string Name => ProviderName;

        protected IConfiguration Configuration { get; }

        public ConfigurationSettingValueProvider(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public virtual Task<string> GetOrNullAsync(SettingDefinition setting)
        {
            return Task.FromResult(Configuration[ConfigurationNamePrefix + setting.Name]);
        }

        public Task<List<SettingValue>> GetAllAsync(SettingDefinition[] settings)
        {
            return Task.FromResult(settings.Select(x => new SettingValue(x.Name, Configuration[ConfigurationNamePrefix + x.Name])).ToList());
        }
    }
}
