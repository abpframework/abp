using Volo.Abp.Modularity;

namespace Volo.Abp.Http.Client.IdentityModel
{
    [DependsOn(
        typeof(AbpHttpClientModule)
        )]
    public class AbpHttpClientIdentityModelModule : AbpModule
    {

    }
}
