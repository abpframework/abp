using ProductManagement;
using Volo.Abp.Autofac;
using Volo.Abp.Http.Client.IdentityModel;
using Volo.Abp.Identity;
using Volo.Abp.Modularity;

namespace ConsoleClientDemo
{
    [DependsOn(
        typeof(AbpAutofacModule),
        typeof(AbpHttpClientIdentityModelModule),
        typeof(AbpIdentityHttpApiClientModule),
        typeof(ProductManagementHttpApiClientModule)
        )]
    public class ConsoleClientDemoModule : AbpModule
    {
        
    }
}
