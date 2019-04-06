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
            Configure<ClientSimulationOptions>(options =>
            {
                //TODO: Temporary add a DemoScenario
                options.Scenarios.Add(
                    new ScenarioConfiguration(
                        typeof(DemoScenario),
                        clientCount: 20
                    )
                );
            });
        }
    }
}
