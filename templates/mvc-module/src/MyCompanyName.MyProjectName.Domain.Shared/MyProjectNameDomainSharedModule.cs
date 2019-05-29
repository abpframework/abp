using Volo.Abp.Modularity;
using Volo.Abp.Localization;
using MyCompanyName.MyProjectName.Localization;
using Volo.Abp.Localization.ExceptionHandling;
using Volo.Abp.Localization.Resources.AbpValidation;
using Volo.Abp.VirtualFileSystem;

namespace MyCompanyName.MyProjectName
{
    [DependsOn(
        typeof(LocalizationModule)
    )]
    public class MyProjectNameDomainSharedModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<VirtualFileSystemOptions>(options =>
            {
                options.FileSets.AddEmbedded<MyProjectNameDomainSharedModule>();
            });

            Configure<AbpLocalizationOptions>(options =>
            {
                options.Resources
                    .Add<MyProjectNameResource>("en")
                    .AddBaseTypes(typeof(AbpValidationResource))
                    .AddVirtualJson("/Localization/MyProjectName/DomainShared");
            });

            Configure<ExceptionLocalizationOptions>(options =>
            {
                options.MapCodeNamespace("MyProjectName", typeof(MyProjectNameResource));
            });
        }
    }
}
