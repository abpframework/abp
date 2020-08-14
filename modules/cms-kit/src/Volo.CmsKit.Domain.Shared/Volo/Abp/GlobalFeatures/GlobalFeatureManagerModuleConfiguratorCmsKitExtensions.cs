using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using Volo.CmsKit.GlobalFeatures;

namespace Volo.Abp.GlobalFeatures
{
    public static class GlobalFeatureManagerModuleConfiguratorCmsKitExtensions
    {
        public static GlobalCmsKitFeatures CmsKit(
            [NotNull] this GlobalModuleFeaturesDictionary modulesFeatures)
        {
            Check.NotNull(modulesFeatures, nameof(modulesFeatures));

            return modulesFeatures
                    .GetOrAdd(GlobalCmsKitFeatures.ModuleName, _ => new GlobalCmsKitFeatures(modulesFeatures.FeatureManager))
                as GlobalCmsKitFeatures;
        }

        public static GlobalModuleFeaturesDictionary CmsKit(
            [NotNull] this GlobalModuleFeaturesDictionary modulesFeatures,
            [NotNull] Action<GlobalCmsKitFeatures> configureAction)
        {
            Check.NotNull(configureAction, nameof(configureAction));

            configureAction(modulesFeatures.CmsKit());

            return modulesFeatures;
        }
    }
}
