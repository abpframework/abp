using Volo.Abp.IdentityModel;
using Volo.Abp.Modularity;

namespace Volo.Abp.Http.Client.IdentityModel.Relay
{
    [DependsOn(
        typeof(AbpHttpClientIdentityModelModule)
        )]
    public class AbpHttpClientIdentityModelRelayModule : AbpModule
    {

    }
}
