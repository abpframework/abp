using System.Collections.Concurrent;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace Volo.Abp.GlobalFeatures
{
    public class GlobalFeatureManager //TODO: Move to the ABP Framework..? //rename?
    {
        public static GlobalFeatureManager Instance { get; protected set; } = new GlobalFeatureManager();

        [NotNull]
        public ConcurrentDictionary<object, object> Configuration { get; }

        protected HashSet<string> EnabledFeatures { get; }

        private GlobalFeatureManager()
        {
            EnabledFeatures = new HashSet<string>();
            Configuration = new ConcurrentDictionary<object, object>();
        }

        public virtual bool IsEnabled(string featureName)
        {
            return EnabledFeatures.Contains(featureName);
        }

        public virtual void SetEnabled(string featureName, bool isEnabled)
        {
            if (isEnabled)
            {
                Enable(featureName);
            }
            else
            {
                Disable(featureName);
            }
        }

        public virtual void Enable(string featureName)
        {
            EnabledFeatures.AddIfNotContains(featureName);
        }

        public virtual void Disable(string featureName)
        {
            EnabledFeatures.Remove(featureName);
        }

        public virtual IEnumerable<string> GetEnabledFeatures()
        {
            return EnabledFeatures;
        }
    }
}
