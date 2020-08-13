using System.Collections.Concurrent;
using JetBrains.Annotations;

namespace Volo.Abp.GlobalFeatures
{
    public class GlobalFeatureManagerModulesConfigurator //TODO: Change to GlobalFeatureManagerModuleDictionary and inherit from ConcurrentDictionary
    {
        public GlobalFeatureManager FeatureManager { get; }

        [NotNull]
        public ConcurrentDictionary<string, GlobalFeatureManagerModuleConfigurator> Modules { get; }

        public GlobalFeatureManagerModulesConfigurator([NotNull] GlobalFeatureManager featureManager)
        {
            FeatureManager = Check.NotNull(featureManager, nameof(featureManager));
            Modules = new ConcurrentDictionary<string, GlobalFeatureManagerModuleConfigurator>();
        }
    }
}
