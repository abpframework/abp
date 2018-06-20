using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Application;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Volo.Abp.TenantManagement.Localization;
using Volo.Abp.VirtualFileSystem;

namespace Volo.Abp.TenantManagement
{
    [DependsOn(typeof(AbpDddApplicationModule))]
    [DependsOn(typeof(AbpTenantManagementDomainSharedModule))]
    public class AbpTenantManagementApplicationContractsModule : AbpModule
    {
        public override void ConfigureServices(IServiceCollection services)
        {
            services.Configure<PermissionOptions>(options =>
            {
                options.DefinitionProviders.Add<AbpTenantManagementPermissionDefinitionProvider>();
            });

            services.Configure<VirtualFileSystemOptions>(options =>
            {
                options.FileSets.AddEmbedded<AbpTenantManagementApplicationContractsModule>();
            });

            services.Configure<AbpLocalizationOptions>(options =>
            {
                options.Resources
                    .Get<AbpTenantManagementResource>()
                    .AddVirtualJson("/Volo/Abp/TenantManagement/Localization/ApplicationContracts");
            });

            services.AddAssemblyOf<AbpTenantManagementApplicationContractsModule>();
        }
    }
}