using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.ApiVersioning;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Volo.Abp.ObjectMapping;
using Volo.Abp.Reflection;
using Volo.Abp.Uow;
using Volo.Abp.Validation;

namespace Volo.Abp
{
    public class AbpKernelModule : AbpModule
    {
        public override void PreConfigureServices(IServiceCollection services)
        {
            services.OnRegistred(UnitOfWorkInterceptorRegistrar.RegisterIfNeeded);
            services.OnRegistred(ValidationInterceptorRegistrar.RegisterIfNeeded);
            
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
            services.AddLocalization();

            AbpStringLocalizerFactory.Replace(services);

            services.AddAssemblyOf<AbpKernelModule>();

            services.AddSingleton<IRequestedApiVersion>(NullRequestedApiVersion.Instance);
            
            services.Configure<ModuleLifecycleOptions>(options =>
            {
                options.Contributers.Add<OnPreApplicationInitializationModuleLifecycleContributer>();
                options.Contributers.Add<OnApplicationInitializationModuleLifecycleContributer>();
                options.Contributers.Add<OnPostApplicationInitializationModuleLifecycleContributer>();
                options.Contributers.Add<OnApplicationShutdownModuleLifecycleContributer>();
            });
        }
    }
}
