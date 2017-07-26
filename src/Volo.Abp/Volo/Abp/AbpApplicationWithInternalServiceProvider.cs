using System;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Modularity;

namespace Volo.Abp
{
    internal class AbpApplicationWithInternalServiceProvider : AbpApplicationBase, IAbpApplicationWithInternalServiceProvider
    {
        public IServiceScope ServiceScope { get; private set; }

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
            Services.AddSingleton<IAbpApplicationWithInternalServiceProvider>(_ => this);
        }

        public void Initialize()
        {
            ServiceScope = Services.BuildServiceProviderFromFactory().CreateScope();
            ServiceProvider = ServiceScope.ServiceProvider;
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