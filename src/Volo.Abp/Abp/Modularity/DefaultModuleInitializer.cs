using System;

namespace Volo.Abp.Modularity
{
    public class DefaultModuleInitializer : IModuleInitializer
    {
        private readonly IServiceProvider _serviceProvider;

        public DefaultModuleInitializer(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public void Initialize(IAbpModule module)
        {
            var context = new ApplicationInitializationContext(
                _serviceProvider
            );

            (module as IOnApplicationInitialization)?.OnApplicationInitialization(context);
        }
    }
}