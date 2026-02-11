using Volo.Abp.Application;
using Volo.Abp.Authorization;
using Volo.Abp.Caching;
using Volo.Abp.Modularity;
using Volo.Abp.ObjectExtending;
using Volo.Abp.ObjectExtending.Modularity;
using Volo.Abp.Threading;
using Volo.CmsKit.Blogs;
using Volo.CmsKit.Contents;
using Volo.CmsKit.Menus;
using Volo.CmsKit.Tags;
using Volo.CmsKit.Users;

namespace Volo.CmsKit;

[DependsOn(
    typeof(CmsKitDomainSharedModule),
    typeof(AbpDddApplicationContractsModule),
    typeof(AbpAuthorizationModule),
    typeof(AbpCachingModule)
)]
public class CmsKitCommonApplicationContractsModule : AbpModule
{
    private readonly static OneTimeRunner OneTimeRunner = new OneTimeRunner();
    
    public override void PostConfigureServices(ServiceConfigurationContext context)
    {
        OneTimeRunner.Run(() =>
        {
            ModuleExtensionConfigurationHelper.ApplyEntityConfigurationToApi(
                CmsKitModuleExtensionConsts.ModuleName,
                CmsKitModuleExtensionConsts.EntityNames.BlogFeature,
                getApiTypes: new[] { typeof(BlogFeatureDto) }
            );

            ModuleExtensionConfigurationHelper.ApplyEntityConfigurationToApi(
                CmsKitModuleExtensionConsts.ModuleName,
                CmsKitModuleExtensionConsts.EntityNames.BlogPost,
                getApiTypes: new[] { typeof(BlogPostCommonDto) }
            );
            
            ModuleExtensionConfigurationHelper.ApplyEntityConfigurationToApi(
                CmsKitModuleExtensionConsts.ModuleName,
                CmsKitModuleExtensionConsts.EntityNames.MenuItem,
                getApiTypes: new[] { typeof(MenuItemDto) }
            );
            
            ModuleExtensionConfigurationHelper.ApplyEntityConfigurationToApi(
                CmsKitModuleExtensionConsts.ModuleName,
                CmsKitModuleExtensionConsts.EntityNames.Tag,
                getApiTypes: new[] { typeof(TagDto) }
            );
            
            ModuleExtensionConfigurationHelper.ApplyEntityConfigurationToApi(
                CmsKitModuleExtensionConsts.ModuleName,
                CmsKitModuleExtensionConsts.EntityNames.CmsUser,
                getApiTypes: new[] { typeof(CmsUserDto) }
            );
        });
    }
}
