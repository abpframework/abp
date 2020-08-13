using JetBrains.Annotations;

namespace Volo.Abp.GlobalFeatures
{
    public static class GlobalFeatureManagerExtensions
    {
        public static GlobalFeatureManagerModulesConfigurator Modules(
            [NotNull] this GlobalFeatureManager featureManager)
        {
            Check.NotNull(featureManager, nameof(featureManager));

            return featureManager
                    .Configuration
                    .GetOrAdd("_Modules", _ => new GlobalFeatureManagerModulesConfigurator(featureManager))
                as GlobalFeatureManagerModulesConfigurator;
        }
    }
}
