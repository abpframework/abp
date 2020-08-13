using System;
using JetBrains.Annotations;

namespace Volo.Abp.GlobalFeatures
{
    public static class GlobalFeatureManagerCmsKitConfiguratorExtensions
    {
        public static GlobalFeature Reactions(
            [NotNull] this GlobalFeatureManagerCmsKitConfigurator cmsKitConfigurator)
        {
            Check.NotNull(cmsKitConfigurator, nameof(cmsKitConfigurator));

            return cmsKitConfigurator
                    .Features
                    .GetOrAdd("Reactions", _ => new GlobalFeature(cmsKitConfigurator, "CmsKit:Reactions"))
                as GlobalFeature;
        }

        public static GlobalFeatureManagerCmsKitConfigurator Reactions(
            [NotNull] this GlobalFeatureManagerCmsKitConfigurator cmsKitConfigurator,
            [NotNull] Action<GlobalFeature> configureAction)
        {
            Check.NotNull(cmsKitConfigurator, nameof(cmsKitConfigurator));

            configureAction(cmsKitConfigurator.Reactions());

            return cmsKitConfigurator;
        }

        public static GlobalFeature Comments(
            [NotNull] this GlobalFeatureManagerCmsKitConfigurator cmsKitConfigurator)
        {
            Check.NotNull(cmsKitConfigurator, nameof(cmsKitConfigurator));

            return cmsKitConfigurator
                    .Features
                    .GetOrAdd("Comments", _ => new GlobalFeature(cmsKitConfigurator, "CmsKit:Comments"))
                as GlobalFeature;
        }

        public static GlobalFeatureManagerCmsKitConfigurator Comments(
            [NotNull] this GlobalFeatureManagerCmsKitConfigurator cmsKitConfigurator,
            [NotNull] Action<GlobalFeature> configureAction)
        {
            Check.NotNull(cmsKitConfigurator, nameof(cmsKitConfigurator));

            configureAction(cmsKitConfigurator.Comments());

            return cmsKitConfigurator;
        }
    }
}
