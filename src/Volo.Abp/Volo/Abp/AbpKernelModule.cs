using System;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Modularity;
using Volo.Abp.ObjectMapping;
using Volo.Abp.Reflection;
using Volo.Abp.Uow;

namespace Volo.Abp
{
    public class AbpKernelModule : AbpModule
    {
        public override void PreConfigureServices(IServiceCollection services)
        {
            services.OnRegistred(UnitOfWorkInterceptorRegistrar.RegisterIfNeeded);
            
            //TODO: Move to a dedicated class
            services.OnExposing(context =>
            {
                //Register types for IObjectMapper<TSource, TDestination> if implements
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
