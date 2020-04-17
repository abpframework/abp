using Volo.Abp.DependencyInjection;

namespace Volo.Abp.Settings
{
    public abstract class SettingDefinitionProvider : ISettingDefinitionProvider, ITransientDependency
    {
        public abstract void Define(ISettingDefinitionContext context);
    }
}