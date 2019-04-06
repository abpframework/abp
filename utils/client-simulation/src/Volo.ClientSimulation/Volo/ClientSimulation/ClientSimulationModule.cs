using Volo.Abp.Http.Client.IdentityModel;
using Volo.Abp.Modularity;
using Volo.ClientSimulation.Scenarios;

namespace Volo.ClientSimulation
{
    [DependsOn(
        typeof(AbpHttpClientIdentityModelModule)
        )]
    public class ClientSimulationModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            //TODO: Temporary add a DemoScenario, remove later
            Configure<ClientSimulationOptions>(options =>
            {
                options.Scenarios.Add(new DemoScenario());
            });
        }
    }
}
