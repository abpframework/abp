using System;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Modularity;
using Volo.DependencyInjection;

namespace Volo.Abp
{
    public class AbpKernelModule : AbpModule
    {
        public override void ConfigureServices(IServiceCollection services)
        {
            services.AddOptions();
            services.AddLogging();

            services.AddAssemblyOf<AbpKernelModule>();

            services.TryAddObjectAccessor<IServiceProvider>();
        }

        public override void OnApplicationInitialization(ApplicationInitializationContext context)
        {
            context.ServiceProvider.GetRequiredService<ObjectAccessor<IServiceProvider>>().Value = context.ServiceProvider;
        }
    }
}
