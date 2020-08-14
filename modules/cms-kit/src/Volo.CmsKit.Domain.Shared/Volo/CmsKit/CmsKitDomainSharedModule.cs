using Volo.Abp.GlobalFeatures;
using Volo.Abp.Modularity;
using Volo.Abp.Localization;
using Volo.CmsKit.Localization;
using Volo.Abp.Localization.ExceptionHandling;
using Volo.Abp.Validation;
using Volo.Abp.Validation.Localization;
using Volo.Abp.VirtualFileSystem;
using Volo.CmsKit.GlobalFeatures;

namespace Volo.CmsKit
{
    [DependsOn(
        typeof(AbpValidationModule)
    )]
    public class CmsKitDomainSharedModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            GlobalFeatureManager.Instance.Modules.CmsKit().EnableAll();
            GlobalFeatureManager.Instance.Modules.CmsKit().DisableAll();
            GlobalFeatureManager.Instance.Modules.CmsKit().Reactions.Enable();
            GlobalFeatureManager.Instance.Modules.CmsKit().Reactions.Enable();
            GlobalFeatureManager.Instance.Modules.CmsKit(cmsKit =>
            {
                cmsKit.Reactions.Disable();
                cmsKit.Comments.Enable();
            });

            GlobalFeatureManager.Instance.IsEnabled("qweasd");
            GlobalFeatureManager.Instance.IsEnabled<ReactionsFeature>();

            Configure<AbpVirtualFileSystemOptions>(options =>
            {
                options.FileSets.AddEmbedded<CmsKitDomainSharedModule>();
            });

            Configure<AbpLocalizationOptions>(options =>
            {
                options.Resources
                    .Add<CmsKitResource>("en")
                    .AddBaseTypes(typeof(AbpValidationResource))
                    .AddVirtualJson("Volo/CmsKit/Localization/Resources");
            });

            Configure<AbpExceptionLocalizationOptions>(options =>
            {
                options.MapCodeNamespace("CmsKit", typeof(CmsKitResource));
            });
        }
    }
}
