using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using Volo.CmsKit.GlobalFeatures;

namespace Volo.Abp.GlobalFeatures;

public static class GlobalModuleFeaturesDictionaryCmsKitExtensions
{
    public static GlobalCmsKitFeatures CmsKit(
        [NotNull] this GlobalModuleFeaturesDictionary modules)
    {
        Check.NotNull(modules, nameof(modules));

        return modules
                .GetOrAdd(
                    GlobalCmsKitFeatures.ModuleName,
                    _ => new GlobalCmsKitFeatures(modules.FeatureManager)
                )
            as GlobalCmsKitFeatures;
    }

    public static GlobalModuleFeaturesDictionary CmsKit(
        [NotNull] this GlobalModuleFeaturesDictionary modules,
        [NotNull] Action<GlobalCmsKitFeatures> configureAction)
    {
        Check.NotNull(configureAction, nameof(configureAction));

        configureAction(modules.CmsKit());

        return modules;
    }
}
