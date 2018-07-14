using Microsoft.Extensions.DependencyInjection;
using MyCompanyName.MyProjectName.Localization;
using Volo.Abp.Application;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace MyCompanyName.MyProjectName
{
    [DependsOn(
        typeof(MyProjectNameDomainSharedModule),
        typeof(AbpDddApplicationModule)
        )]
    public class MyProjectNameApplicationContractsModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.Configure<PermissionOptions>(options =>
            {
                options.DefinitionProviders.Add<MyProjectNamePermissionDefinitionProvider>();
            });

            context.Services.Configure<VirtualFileSystemOptions>(options =>
            {
                options.FileSets.AddEmbedded<MyProjectNameApplicationContractsModule>();
            });

            context.Services.Configure<AbpLocalizationOptions>(options =>
            {
                options.Resources
                    .Get<MyProjectNameResource>()
                    .AddVirtualJson("/MyCompanyName/MyProjectName/Localization/ApplicationContracts");
            });

            context.Services.AddAssemblyOf<MyProjectNameApplicationContractsModule>();
        }
    }
}
