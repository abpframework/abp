using Volo.Abp.BlobStoring;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using Volo.Abp;
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
        typeof(AbpDddDomainModule),
        typeof(AbpBlobStoringModule)
    )]
    public class CmsKitDomainModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<CmsKitReactionOptions>(options =>
            {
                options.Reactions.AddIfNotContains(new ReactionDefinition(StandardReactions.ThumbsUp));
                options.Reactions.AddIfNotContains(new ReactionDefinition(StandardReactions.ThumbsDown));
                options.Reactions.AddIfNotContains(new ReactionDefinition(StandardReactions.Smile));
                options.Reactions.AddIfNotContains(new ReactionDefinition(StandardReactions.Wink));
                options.Reactions.AddIfNotContains(new ReactionDefinition(StandardReactions.Confused));
                options.Reactions.AddIfNotContains(new ReactionDefinition(StandardReactions.Victory));
                options.Reactions.AddIfNotContains(new ReactionDefinition(StandardReactions.Rock));
                options.Reactions.AddIfNotContains(new ReactionDefinition(StandardReactions.Eyes));
                options.Reactions.AddIfNotContains(new ReactionDefinition(StandardReactions.Heart));
                options.Reactions.AddIfNotContains(new ReactionDefinition(StandardReactions.HeartBroken));
                options.Reactions.AddIfNotContains(new ReactionDefinition(StandardReactions.Rocket));
                options.Reactions.AddIfNotContains(new ReactionDefinition(StandardReactions.Pray));

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