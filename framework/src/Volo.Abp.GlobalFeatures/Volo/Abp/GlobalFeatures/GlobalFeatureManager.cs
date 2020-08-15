﻿using System;
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

        internal GlobalFeatureManager()
        {
            EnabledFeatures = new HashSet<string>();
            Configuration = new Dictionary<object, object>();
            Modules = new GlobalModuleFeaturesDictionary(this);
        }

        public virtual bool IsEnabled<TFeature>()
            where TFeature : GlobalFeature
        {
            return IsEnabled(GlobalFeatureNameAttribute.GetName<TFeature>());
        }

        public virtual bool IsEnabled([NotNull] Type featureType)
        {
            return IsEnabled(GlobalFeatureNameAttribute.GetName(featureType));
        }

        public virtual bool IsEnabled(string featureName)
        {
            return EnabledFeatures.Contains(featureName);
        }

        protected internal void Enable(string featureName)
        {
            EnabledFeatures.AddIfNotContains(featureName);
        }

        protected internal void Disable(string featureName)
        {
            EnabledFeatures.Remove(featureName);
        }

        public virtual IEnumerable<string> GetEnabledFeatureNames()
        {
            return EnabledFeatures;
        }
    }
}
