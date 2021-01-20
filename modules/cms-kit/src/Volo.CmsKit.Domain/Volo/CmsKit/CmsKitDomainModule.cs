using Microsoft.Extensions.Options;
using Volo.Abp.Domain;
using Volo.Abp.GlobalFeatures;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Volo.Abp.Users;
using Volo.CmsKit.GlobalFeatures;
using Volo.CmsKit.Localization;
using Volo.CmsKit.Pages;
using Volo.CmsKit.Reactions;
using Volo.CmsKit.Tags;

namespace Volo.CmsKit
{
    [DependsOn(
        typeof(CmsKitDomainSharedModule),
        typeof(AbpUsersDomainModule),
        typeof(AbpDddDomainModule)
    )]
    public class CmsKitDomainModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<CmsKitOptions>(options =>
            {
                options.Reactions.AddOrReplace(StandardReactions.ThumbsUp);
                options.Reactions.AddOrReplace(StandardReactions.ThumbsDown);
                options.Reactions.AddOrReplace(StandardReactions.Smile);
                options.Reactions.AddOrReplace(StandardReactions.Wink);
                options.Reactions.AddOrReplace(StandardReactions.Confused);
                options.Reactions.AddOrReplace(StandardReactions.Victory);
                options.Reactions.AddOrReplace(StandardReactions.Rock);
                options.Reactions.AddOrReplace(StandardReactions.Eyes);
                options.Reactions.AddOrReplace(StandardReactions.Heart);
                options.Reactions.AddOrReplace(StandardReactions.HeartBroken);
                options.Reactions.AddOrReplace(StandardReactions.Rocket);
                options.Reactions.AddOrReplace(StandardReactions.Pray);

            });

            if (GlobalFeatureManager.Instance.IsEnabled<TagsFeature>())
            {
                // TODO: Configure TagEntityTypes here...
            }
        }

        private static LocalizableString L(string name)
        {
            return LocalizableString.Create<CmsKitResource>(name);
        }
    }
}