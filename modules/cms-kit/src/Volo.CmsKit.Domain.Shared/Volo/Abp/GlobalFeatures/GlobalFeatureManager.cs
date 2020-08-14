using System.Collections.Concurrent;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace Volo.Abp.GlobalFeatures
{
    public class GlobalFeatureManager
    {
        public static GlobalFeatureManager Instance { get; protected set; } = new GlobalFeatureManager();

        /// <summary>
        /// A common dictionary to store arbitrary configurations.
        /// </summary>
        [NotNull]
        public Dictionary<object, object> Configuration { get; }

        public GlobalFeatureManagerModuleDictionary Modules { get; }

        protected HashSet<string> EnabledFeatures { get; }

        private GlobalFeatureManager()
        {
            EnabledFeatures = new HashSet<string>();
            Configuration = new Dictionary<object, object>();
            Modules = new GlobalFeatureManagerModuleDictionary(this);
        }

        public virtual bool IsEnabled<TFeature>()
            where TFeature : GlobalFeature
        {
            return IsEnabled(GlobalFeatureNameAttribute.GetName<TFeature>());
        }

        public virtual void SetEnabled<TFeature>(bool isEnabled)
            where TFeature : GlobalFeature
        {
            SetEnabled(GlobalFeatureNameAttribute.GetName<TFeature>(), isEnabled);
        }

        public virtual void Enable<TFeature>()
            where TFeature : GlobalFeature
        {
            Enable(GlobalFeatureNameAttribute.GetName<TFeature>());
        }

        public virtual void Disable<TFeature>()
            where TFeature : GlobalFeature
        {
            Disable(GlobalFeatureNameAttribute.GetName<TFeature>());
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

        public virtual IEnumerable<string> GetEnabledFeatureNames()
        {
            return EnabledFeatures;
        }
    }
}
