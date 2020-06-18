using ProductManagement;
using Volo.Abp.Autofac;
using Volo.Abp.Http.Client.IdentityModel;
using Volo.Abp.Identity;
using Volo.Abp.Modularity;
using Volo.Abp.TenantManagement;

namespace ConsoleClientDemo
{
    [DependsOn(
        typeof(AbpAutofacModule),
        typeof(AbpHttpClientIdentityModelModule),
        typeof(AbpIdentityHttpApiClientModule),
        typeof(ProductManagementHttpApiClientModule),
        typeof(AbpTenantManagementHttpApiClientModule)
        )]
    public class ConsoleClientDemoModule : AbpModule
    {
         
    }
}
