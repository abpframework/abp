using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Modularity;

namespace Volo.Abp.Tests.Modularity
{
    public class IndependentEmptyModule : AbpModule
    {
        public bool ConfigureServicesIsCalled { get; set; }

        public bool OnApplicationInitializeIsCalled { get; set; }

        public override void ConfigureServices(IServiceCollection services)
        {
            ConfigureServicesIsCalled = true;
        }

        public override void OnApplicationInitialization(ApplicationInitializationContext context)
        {
            OnApplicationInitializeIsCalled = true;
        }
    }
}
