using Volo.Abp.DependencyInjection;

namespace Volo.Abp.Settings
{
    public abstract class SettingProvider : ISettingProvider, ISingletonDependency
    {
        public abstract void Define(ISettingDefinitionContext context);
    }
}