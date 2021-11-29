using System;
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

        public GlobalModuleFeaturesDictionary Modules { get; }

        protected HashSet<string> EnabledFeatures { get; }

        protected internal GlobalFeatureManager()
        {
            EnabledFeatures = new HashSet<string>();
            Configuration = new Dictionary<object, object>();
            Modules = new GlobalModuleFeaturesDictionary(this);
        }

        public virtual bool IsEnabled<TFeature>()
        {
            return IsEnabled(typeof(TFeature));
        }

        public virtual bool IsEnabled([NotNull] Type featureType)
        {
            return IsEnabled(GlobalFeatureNameAttribute.GetName(featureType));
        }

        public virtual bool IsEnabled(string featureName)
        {
            return EnabledFeatures.Contains(featureName);
        }
        
        public virtual void Enable<TFeature>()
        {
            Enable(typeof(TFeature));
        }
        
        public virtual void Enable([NotNull] Type featureType)
        {
            Enable(GlobalFeatureNameAttribute.GetName(featureType));
        }

        public virtual void Enable(string featureName)
        {
            EnabledFeatures.AddIfNotContains(featureName);
        }
        
        public virtual void Disable<TFeature>()
        {
            Disable(typeof(TFeature));
        }
        
        public virtual void Disable([NotNull] Type featureType)
        {
            Disable(GlobalFeatureNameAttribute.GetName(featureType));
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
