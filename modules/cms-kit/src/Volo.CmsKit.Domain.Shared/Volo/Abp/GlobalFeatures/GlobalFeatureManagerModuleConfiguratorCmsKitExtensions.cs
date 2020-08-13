using System;
using JetBrains.Annotations;

namespace Volo.Abp.GlobalFeatures
{
    public static class GlobalFeatureManagerModuleConfiguratorCmsKitExtensions
    {
        public static GlobalFeatureManagerCmsKitConfigurator CmsKit(
            [NotNull] this GlobalFeatureManagerModulesConfigurator modulesConfigurator)
        {
            Check.NotNull(modulesConfigurator, nameof(modulesConfigurator));

            return modulesConfigurator
                    .Modules
                    .GetOrAdd("CmsKit", _ => new GlobalFeatureManagerCmsKitConfigurator(modulesConfigurator))
                as GlobalFeatureManagerCmsKitConfigurator;
        }

        public static GlobalFeatureManagerModulesConfigurator CmsKit(
            [NotNull] this GlobalFeatureManagerModulesConfigurator modulesConfigurator,
            [NotNull] Action<GlobalFeatureManagerCmsKitConfigurator> configureAction)
        {
            Check.NotNull(configureAction, nameof(configureAction));

            configureAction(modulesConfigurator.CmsKit());

            return modulesConfigurator;
        }
    }
}
