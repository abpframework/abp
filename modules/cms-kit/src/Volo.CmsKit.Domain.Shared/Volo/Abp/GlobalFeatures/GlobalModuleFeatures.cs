using JetBrains.Annotations;

namespace Volo.Abp.GlobalFeatures
{
    public abstract class GlobalModuleFeatures
    {
        [NotNull]
        public GlobalFeatureManager FeatureManager { get; }

        [NotNull]
        protected GlobalFeatureDictionary AllFeatures { get; }

        protected GlobalModuleFeatures(
            [NotNull] GlobalFeatureManager featureManager)
        {
            FeatureManager = Check.NotNull(featureManager, nameof(featureManager));
            AllFeatures = new GlobalFeatureDictionary();
        }

        public virtual void EnableAll()
        {
            foreach (var feature in AllFeatures.Values)
            {
                feature.Enable();
            }
        }

        public virtual void DisableAll()
        {
            foreach (var feature in AllFeatures.Values)
            {
                feature.Disable();
            }
        }

        protected void AddFeature(GlobalFeature feature)
        {
            AllFeatures[feature.FeatureName] = feature;
        }

        protected GlobalFeature GetFeature(string featureName)
        {
            return AllFeatures[featureName];
        }

        protected TFeature GetFeature<TFeature>()
            where TFeature : GlobalFeature
        {
            return (TFeature) GetFeature(GlobalFeatureNameAttribute.GetName<TFeature>());
        }
    }
}
