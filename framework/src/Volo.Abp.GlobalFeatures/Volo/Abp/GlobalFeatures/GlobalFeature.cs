using JetBrains.Annotations;

namespace Volo.Abp.GlobalFeatures;

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
        set => SetEnabled(value);
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

    public void SetEnabled(bool isEnabled)
    {
        if (isEnabled)
        {
            Enable();
        }
        else
        {
            Disable();
        }
    }
}
