using Volo.Abp.AspNetCore.Mvc.UI.Theme.Shared;
using Volo.Abp.AutoMapper;
using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;
using Volo.CmsKit.Reactions;
using Volo.CmsKit.Web.Icons;

namespace Volo.CmsKit.Web
{
    [DependsOn(
        typeof(AbpAspNetCoreMvcUiThemeSharedModule),
        typeof(CmsKitCommonHttpApiModule),
        typeof(AbpAutoMapperModule)
        )]
    public class CmsKitCommonWebModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<CmsKitUiOptions>(options =>
            {
                options.ReactionIcons[StandardReactions.Smile] = new LocalizableIconDictionary("/cms-kit/icons/smile.png");
                options.ReactionIcons[StandardReactions.ThumbsUp] = new LocalizableIconDictionary("/cms-kit/icons/thumbsup.png");
                options.ReactionIcons[StandardReactions.Confused] = new LocalizableIconDictionary("/cms-kit/icons/confused.png");
                options.ReactionIcons[StandardReactions.Eyes] = new LocalizableIconDictionary("/cms-kit/icons/eyes.png");
                options.ReactionIcons[StandardReactions.Heart] = new LocalizableIconDictionary("/cms-kit/icons/heart.png");
                options.ReactionIcons[StandardReactions.Hooray] = new LocalizableIconDictionary("/cms-kit/icons/hooray.png");
                options.ReactionIcons[StandardReactions.Rocket] = new LocalizableIconDictionary("/cms-kit/icons/rocket.png");
                options.ReactionIcons[StandardReactions.ThumbsDown] = new LocalizableIconDictionary("/cms-kit/icons/thumbsdown.png");
            });

            Configure<AbpVirtualFileSystemOptions>(options =>
            {
                options.FileSets.AddEmbedded<CmsKitCommonWebModule>();
            });
        }
    }
}
