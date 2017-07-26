using System;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Modularity;

namespace Volo.Abp
{
    internal class AbpApplicationWithExternalServiceProvider : AbpApplicationBase, IAbpApplicationWithExternalServiceProvider
    {
        public AbpApplicationWithExternalServiceProvider(
            [NotNull] Type startupModuleType, 
            [NotNull] IServiceCollection services, 
            [CanBeNull] Action<AbpApplicationCreationOptions> optionsAction
            ) : base(
                startupModuleType, 
                services, 
                optionsAction)
        {
            services.AddSingleton<IAbpApplicationWithExternalServiceProvider>(_ => this);
        }

        public void Initialize(IServiceProvider serviceProvider)
        {
            Check.NotNull(serviceProvider, nameof(serviceProvider));

            ServiceProvider = serviceProvider;

            ServiceProvider
                .GetRequiredService<IModuleManager>()
                .InitializeModules(new ApplicationInitializationContext(ServiceProvider));
        }
    }
}
