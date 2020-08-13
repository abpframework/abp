using System.Collections.Concurrent;
using JetBrains.Annotations;

namespace Volo.Abp.GlobalFeatures
{
    public abstract class GlobalFeatureManagerModuleConfigurator
    {
        [NotNull]
        public GlobalFeatureManagerModulesConfigurator ModulesConfigurator { get; }

        [NotNull]
        public ConcurrentDictionary<string, GlobalFeature> Features { get; }

        protected GlobalFeatureManagerModuleConfigurator(GlobalFeatureManagerModulesConfigurator modulesConfigurator)
        {
            ModulesConfigurator = Check.NotNull(modulesConfigurator, nameof(modulesConfigurator));
            Features = new ConcurrentDictionary<string, GlobalFeature>();
        }

        public virtual void EnableAll()
        {
            foreach (var feature in Features.Values)
            {
                feature.Enable();
            }
        }

        public virtual void DisableAll()
        {
            foreach (var feature in Features.Values)
            {
                feature.Disable();
            }
        }
    }
}
