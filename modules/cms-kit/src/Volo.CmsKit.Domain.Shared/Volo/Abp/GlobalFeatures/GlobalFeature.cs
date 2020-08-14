using JetBrains.Annotations;

namespace Volo.Abp.GlobalFeatures
{
    public abstract class GlobalFeature
    {
        [NotNull]
        public GlobalModuleFeatures Module { get; }

        [NotNull]
        public GlobalFeatureManager FeatureManager { get; }

        [NotNull]
        public string FeatureName { get; }

        public bool IsEnabled
        {
            get => FeatureManager.IsEnabled(FeatureName);
            set => FeatureManager.SetEnabled(FeatureName, value);
        }

        protected GlobalFeature([NotNull] GlobalModuleFeatures module)
        {
            Module = Check.NotNull(module, nameof(module));
            FeatureManager = Module.FeatureManager;
            FeatureName = GlobalFeatureNameAttribute.GetName(GetType());
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
            FeatureManager.SetEnabled(FeatureName, isEnabled);
        }
    }
}
