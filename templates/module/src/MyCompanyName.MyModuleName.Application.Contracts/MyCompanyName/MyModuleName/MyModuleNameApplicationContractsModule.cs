using Microsoft.Extensions.DependencyInjection;
using MyCompanyName.MyModuleName.Localization;
using Volo.Abp.Application;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace MyCompanyName.MyModuleName
{
    [DependsOn(
        typeof(MyModuleNameDomainSharedModule),
        typeof(AbpDddApplicationModule)
        )]
    public class MyModuleNameApplicationContractsModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.Configure<PermissionOptions>(options =>
            {
                options.DefinitionProviders.Add<MyModuleNamePermissionDefinitionProvider>();
            });

            context.Services.Configure<VirtualFileSystemOptions>(options =>
            {
                options.FileSets.AddEmbedded<MyModuleNameApplicationContractsModule>();
            });

            context.Services.Configure<AbpLocalizationOptions>(options =>
            {
                options.Resources
                    .Get<MyModuleNameResource>()
                    .AddVirtualJson("/MyCompanyName/MyModuleName/Localization/ApplicationContracts");
            });

            context.Services.AddAssemblyOf<MyModuleNameApplicationContractsModule>();
        }
    }
}
