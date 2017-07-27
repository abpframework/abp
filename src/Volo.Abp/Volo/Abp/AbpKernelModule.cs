using System;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Application.Services;
using Volo.Abp.Modularity;
using Volo.Abp.ObjectMapping;
using Volo.Abp.Reflection;
using Volo.Abp.Uow;
using Volo.DependencyInjection;

namespace Volo.Abp
{
    public class AbpKernelModule : AbpModule
    {
        public override void PreConfigureServices(IServiceCollection services)
        {
            //TODO: Move these to dedicated class(es)

            services.OnServiceRegistred(registration =>
            {
                if (typeof(IApplicationService).GetTypeInfo().IsAssignableFrom(registration.ImplementationType))
                {
                    registration.Interceptors.Add<UnitOfWorkInterceptor>();
                }
            });

            services.OnServiceExposing(context =>
            {
                context.ExposedTypes.AddRange(
                    ReflectionHelper.GetImplementedGenericTypes(
                        context.ImplementationType,
                        typeof(IObjectMapper<,>)
                    )
                );
            });
        }

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
