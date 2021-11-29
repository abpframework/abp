using Microsoft.Extensions.DependencyInjection;

namespace Volo.Abp.Modularity
{
    public class TestModuleBase : AbpModule
    {
        public bool PreConfigureServicesIsCalled { get; set; }

        public bool ConfigureServicesIsCalled { get; set; }

        public bool PostConfigureServicesIsCalled { get; set; }

        public bool OnApplicationInitializeIsCalled { get; set; }

        public bool OnApplicationShutdownIsCalled { get; set; }

        public override void PreConfigureServices(ServiceConfigurationContext context)
        {
            PreConfigureServicesIsCalled = true;
        }

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            ConfigureServicesIsCalled = true;
        }

        public override void PostConfigureServices(ServiceConfigurationContext context)
        {
            PostConfigureServicesIsCalled = true;
        }

        public override void OnApplicationInitialization(ApplicationInitializationContext context)
        {
            OnApplicationInitializeIsCalled = true;
        }

        public override void OnApplicationShutdown(ApplicationShutdownContext context)
        {
            OnApplicationShutdownIsCalled = true;
        }
    }
}