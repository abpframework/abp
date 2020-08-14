using JetBrains.Annotations;

namespace Volo.Abp.GlobalFeatures
{
    public abstract class GlobalModuleFeatures
    {
        [NotNull]
        public GlobalFeatureConfiguratorDictionary AllFeatures { get; }

        [NotNull]
        public GlobalFeatureManager FeatureManager { get; }

        protected GlobalModuleFeatures(
            GlobalFeatureManager featureManager)
        {
            AllFeatures = new GlobalFeatureConfiguratorDictionary();
            FeatureManager = featureManager;
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

        protected void AddFeature(string featureName)
        {
            AddFeature(new GlobalFeature(this, featureName));
        }

        protected void AddFeature(GlobalFeature feature)
        {
            AllFeatures[feature.FeatureName] = feature;
        }

        protected GlobalFeature GetFeature(string featureName)
        {
            return AllFeatures[featureName];
        }

        protected TFeature GetFeature<TFeature>(string featureName)
            where TFeature : GlobalFeature
        {
            return (TFeature) AllFeatures[featureName];
        }
    }
}
