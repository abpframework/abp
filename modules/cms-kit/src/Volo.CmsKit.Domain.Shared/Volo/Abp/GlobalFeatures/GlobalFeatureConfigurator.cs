using JetBrains.Annotations;

namespace Volo.Abp.GlobalFeatures
{
    public class GlobalFeature
    {
        [NotNull]
        public GlobalModuleFeatures Module { get; }

        [NotNull]
        public GlobalFeatureManager FeatureManager { get; }

        public string FeatureName { get; }

        public bool IsEnabled
        {
            get => FeatureManager.IsEnabled(FeatureName);
            set => FeatureManager.SetEnabled(FeatureName, value);
        }

        public GlobalFeature(
            [NotNull] GlobalModuleFeatures module,
            [NotNull] string name)
        {
            Module = Check.NotNull(module, nameof(module));
            FeatureName = Check.NotNullOrWhiteSpace(name, nameof(name));
            FeatureManager = Module.FeatureManager;
        }

        public virtual void Enable()
        {
            FeatureManager.Enable(FeatureName);
        }

        public virtual void Disable()
        {
            FeatureManager.Disable(FeatureName);
        }

        public virtual void SetEnabled(bool isEnabled)
        {

        }
    }
}
