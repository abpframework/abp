using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Volo.Abp.TenantManagement.Localization;

namespace Volo.Abp.TenantManagement
{
    public class AbpTenantManagementDomainSharedModule : AbpModule
    {
        public override void ConfigureServices(IServiceCollection services)
        {
            services.Configure<AbpLocalizationOptions>(options =>
            {
                options.Resources.Add<AbpTenantManagementResource>("en");
            });

            services.AddAssemblyOf<AbpTenantManagementDomainSharedModule>();
        }
    }
}
