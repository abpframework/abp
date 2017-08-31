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
            Services.AddSingleton<IAbpApplicationWithInternalServiceProvider>(this);
        }

        public void Initialize()
        {
            ServiceScope = Services.BuildServiceProviderFromFactory().CreateScope();
            ServiceProvider = ServiceScope.ServiceProvider;

            using (var scope = ServiceProvider.CreateScope())
            {
                ServiceProvider
                    .GetRequiredService<IModuleManager>()
                    .InitializeModules(new ApplicationInitializationContext(scope.ServiceProvider));
            }
        }

        public override void Dispose()
        {
            base.Dispose();
            ServiceScope.Dispose();
        }
    }
}