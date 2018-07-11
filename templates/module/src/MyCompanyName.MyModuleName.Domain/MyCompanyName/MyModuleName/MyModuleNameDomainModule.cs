using Microsoft.Extensions.DependencyInjection;
using MyCompanyName.MyModuleName.Localization;
using Volo.Abp.Localization;
using Volo.Abp.Localization.ExceptionHandling;
using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace MyCompanyName.MyModuleName
{
    [DependsOn(
        typeof(MyModuleNameDomainSharedModule)
        )]
    public class MyModuleNameDomainModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.Configure<VirtualFileSystemOptions>(options =>
            {
                options.FileSets.AddEmbedded<MyModuleNameDomainModule>();
            });

            context.Services.Configure<AbpLocalizationOptions>(options =>
            {
                options.Resources.Get<MyModuleNameResource>().AddVirtualJson("/MyCompanyName/MyModuleName/Localization/Domain");
            });

            context.Services.Configure<ExceptionLocalizationOptions>(options =>
            {
                options.MapCodeNamespace("Volo.Qa", typeof(MyModuleNameResource));
            });

            context.Services.AddAssemblyOf<MyModuleNameDomainModule>();
        }
    }
}
