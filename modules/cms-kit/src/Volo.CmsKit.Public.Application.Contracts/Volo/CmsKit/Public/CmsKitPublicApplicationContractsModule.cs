using Volo.Abp.Modularity;
using Volo.Abp.EventBus;
using Volo.Abp.ObjectExtending;
using Volo.Abp.ObjectExtending.Modularity;
using Volo.Abp.Threading;
using Volo.CmsKit.Public.Comments;
using Volo.CmsKit.Public.GlobalResources;

namespace Volo.CmsKit.Public;

[DependsOn(
    typeof(CmsKitCommonApplicationContractsModule),
    typeof(AbpEventBusModule)
    )]
public class CmsKitPublicApplicationContractsModule : AbpModule
{
    private readonly static OneTimeRunner OneTimeRunner = new OneTimeRunner();
    
    public override void PostConfigureServices(ServiceConfigurationContext context)
    {
        OneTimeRunner.Run(() =>
        {
            ModuleExtensionConfigurationHelper.ApplyEntityConfigurationToApi(
                CmsKitModuleExtensionConsts.ModuleName,
                CmsKitModuleExtensionConsts.EntityNames.Comment,
                getApiTypes: new[] { typeof(CommentDto) , typeof(CommentWithDetailsDto) },
                createApiTypes: new []{ typeof(CreateCommentInput) },
                updateApiTypes: new []{ typeof(UpdateCommentInput) }
            );

            ModuleExtensionConfigurationHelper.ApplyEntityConfigurationToApi(
                CmsKitModuleExtensionConsts.ModuleName,
                CmsKitModuleExtensionConsts.EntityNames.GlobalResource,
                getApiTypes: new[] { typeof(GlobalResourceDto) }
            );
        });
    }
}
