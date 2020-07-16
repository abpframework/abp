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
            });

            Configure<AbpVirtualFileSystemOptions>(options =>
            {
                options.FileSets.AddEmbedded<CmsKitCommonWebModule>();
            });
        }
    }
}
