using System;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;

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
            services.AddSingleton<IAbpApplicationWithExternalServiceProvider>(this);
        }

        public void Initialize(IServiceProvider serviceProvider)
        {
            Check.NotNull(serviceProvider, nameof(serviceProvider));

            SetServiceProvider(serviceProvider);

            InitializeModules();
        }

        public override void Dispose()
        {
            base.Dispose();

            if (ServiceProvider is IDisposable disposableServiceProvider)
            {
                disposableServiceProvider.Dispose();
            }
        }
    }
}
