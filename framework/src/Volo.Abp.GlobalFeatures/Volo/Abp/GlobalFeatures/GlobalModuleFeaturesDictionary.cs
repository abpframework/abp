using System.Collections.Generic;
using JetBrains.Annotations;

namespace Volo.Abp.GlobalFeatures
{
    public class GlobalModuleFeaturesDictionary : Dictionary<string, GlobalModuleFeatures>
    {
        public GlobalFeatureManager FeatureManager { get; }

        public GlobalModuleFeaturesDictionary(
            [NotNull] GlobalFeatureManager featureManager)
        {
            FeatureManager = Check.NotNull(featureManager, nameof(featureManager));
        }
    }
}
