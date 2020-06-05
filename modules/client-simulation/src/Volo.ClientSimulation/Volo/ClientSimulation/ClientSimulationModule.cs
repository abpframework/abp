using Volo.Abp.Http.Client.IdentityModel;
using Volo.Abp.Modularity;

namespace Volo.ClientSimulation
{
    [DependsOn(
        typeof(AbpHttpClientIdentityModelModule)
        )]
    public class ClientSimulationModule : AbpModule
    {
        
    }
}
