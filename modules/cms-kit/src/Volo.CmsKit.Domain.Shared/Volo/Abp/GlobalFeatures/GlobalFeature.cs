using JetBrains.Annotations;

namespace Volo.Abp.GlobalFeatures
{
    public class GlobalFeature
    {
        public GlobalFeatureManagerModuleConfigurator ModuleConfigurator { get; }

        public string Name { get; }

        public bool IsEnabled => ModuleConfigurator.ModulesConfigurator.FeatureManager.IsEnabled(Name);

        public GlobalFeature(
            [NotNull] GlobalFeatureManagerModuleConfigurator moduleConfigurator,
            [NotNull] string name)
        {
            ModuleConfigurator = Check.NotNull(moduleConfigurator, nameof(moduleConfigurator));
            Name = Check.NotNullOrWhiteSpace(name, nameof(name));
        }

        public virtual void Enable()
        {
            ModuleConfigurator.ModulesConfigurator.FeatureManager.Enable(Name);
        }

        public virtual void Disable()
        {
            ModuleConfigurator.ModulesConfigurator.FeatureManager.Disable(Name);
        }

        public virtual void SetEnabled(bool isEnabled)
        {
            ModuleConfigurator.ModulesConfigurator.FeatureManager.SetEnabled(Name, isEnabled);
        }
    }
}
