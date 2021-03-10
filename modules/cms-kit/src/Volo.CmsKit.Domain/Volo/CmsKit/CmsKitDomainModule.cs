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
using Volo.CmsKit.Blogs;
using Volo.CmsKit.Comments;

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
            if (GlobalFeatureManager.Instance.IsEnabled<ReactionsFeature>())
            {
                Configure<CmsKitReactionOptions>(options =>
                {
                    if (GlobalFeatureManager.Instance.IsEnabled<BlogsFeature>())
                    {
                        options.EntityTypes.Add(
                            new ReactionEntityTypeDefinition(
                                BlogPostConsts.EntityType,
                                reactions: new[]
                                {
                                    new ReactionDefinition(StandardReactions.Smile),
                                    new ReactionDefinition(StandardReactions.ThumbsUp),
                                    new ReactionDefinition(StandardReactions.ThumbsDown),
                                    new ReactionDefinition(StandardReactions.Confused),
                                    new ReactionDefinition(StandardReactions.Eyes),
                                    new ReactionDefinition(StandardReactions.Heart),
                                    new ReactionDefinition(StandardReactions.HeartBroken),
                                    new ReactionDefinition(StandardReactions.Wink),
                                    new ReactionDefinition(StandardReactions.Pray),
                                    new ReactionDefinition(StandardReactions.Rocket),
                                    new ReactionDefinition(StandardReactions.Victory),
                                    new ReactionDefinition(StandardReactions.Rock),
                                }));
                    }

                    if (GlobalFeatureManager.Instance.IsEnabled<CommentsFeature>())
                    {
                        options.EntityTypes.Add(
                            new ReactionEntityTypeDefinition(
                                CommentConsts.EntityType,
                                reactions: new[]
                                {
                                    new ReactionDefinition(StandardReactions.ThumbsUp),
                                    new ReactionDefinition(StandardReactions.ThumbsDown),
                                }));
                    }
                });
            }

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