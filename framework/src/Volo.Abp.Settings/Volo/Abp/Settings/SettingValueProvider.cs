using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.Settings
{
    public abstract class SettingValueProvider : ISettingValueProvider, ITransientDependency
    {
        public abstract string Name { get; }

        protected ISettingStore SettingStore { get; }

        protected SettingValueProvider(ISettingStore settingStore)
        {
            SettingStore = settingStore;
        }

        public abstract Task<string> GetOrNullAsync(SettingDefinition setting);
    }
}