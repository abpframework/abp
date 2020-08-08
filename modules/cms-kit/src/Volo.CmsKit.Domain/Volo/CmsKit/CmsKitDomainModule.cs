using Volo.Abp.Domain;
using Volo.Abp.Modularity;
using Volo.Abp.Users;
using Volo.CmsKit.Reactions;

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
                options.Reactions.AddOrReplace(StandardReactions.Smile);
                options.Reactions.AddOrReplace(StandardReactions.ThumbsUp);
                options.Reactions.AddOrReplace(StandardReactions.ThumbsDown);
                options.Reactions.AddOrReplace(StandardReactions.Confused);
                options.Reactions.AddOrReplace(StandardReactions.Eyes);
                options.Reactions.AddOrReplace(StandardReactions.Heart);
                options.Reactions.AddOrReplace(StandardReactions.Hooray);
                options.Reactions.AddOrReplace(StandardReactions.Rocket);
            });
        }
    }
}
