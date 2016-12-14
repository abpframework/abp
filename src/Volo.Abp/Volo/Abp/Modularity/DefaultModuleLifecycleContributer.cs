using System;

namespace Volo.Abp.Modularity
{
    public class DefaultModuleLifecycleContributer : ModuleLifecycleContributerBase
    {
        private readonly IServiceProvider _serviceProvider;

        public DefaultModuleLifecycleContributer(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public override void Initialize(IAbpModule module)
        {
            var context = new ApplicationInitializationContext(
                _serviceProvider
            );

            (module as IOnApplicationInitialization)?.OnApplicationInitialization(context);
        }

        public override void Shutdown(IAbpModule module)
        {
            var context = new ApplicationShutdownContext();

            (module as IOnApplicationShutdown)?.OnApplicationShutdown(context);
        }
    }
}