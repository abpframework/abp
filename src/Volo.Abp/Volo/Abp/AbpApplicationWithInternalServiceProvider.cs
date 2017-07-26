using System;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Modularity;

namespace Volo.Abp
{
    internal class AbpApplicationWithInternalServiceProvider : AbpApplicationBase, IAbpApplicationWithInternalServiceProvider
    {
        public IServiceScope ServiceScope { get; }

        public AbpApplicationWithInternalServiceProvider(
            [NotNull] Type startupModuleType,
            [CanBeNull] Action<AbpApplicationCreationOptions> optionsAction
            ) : this(
            startupModuleType,
            new ServiceCollection(),
            optionsAction)
        {

        }

        private AbpApplicationWithInternalServiceProvider(
            [NotNull] Type startupModuleType, 
            [NotNull] IServiceCollection services, 
            [CanBeNull] Action<AbpApplicationCreationOptions> optionsAction
            ) : base(
                startupModuleType, 
                services, 
                optionsAction)
        {
            services.AddSingleton<IAbpApplicationWithInternalServiceProvider>(_ => this);

            ServiceScope = services.BuildServiceProviderFromFactory().CreateScope();
            ServiceProvider = ServiceScope.ServiceProvider;
        }

        public void Initialize()
        {
            ServiceProvider
                .GetRequiredService<IModuleManager>()
                .InitializeModules(new ApplicationInitializationContext(ServiceProvider));
        }

        public override void Dispose()
        {
            base.Dispose();
            ServiceScope.Dispose();
        }
    }
}