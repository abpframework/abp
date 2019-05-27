using Volo.Abp.Http.Client.IdentityModel;
using Volo.Abp.Modularity;

namespace Volo.ClientSimulation
{
    [DependsOn(
        typeof(HttpClientIdentityModelModule)
        )]
    public class ClientSimulationModule : AbpModule
    {
        
    }
}
