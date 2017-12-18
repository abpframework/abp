using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.ApiVersioning;
using Volo.Abp.Data;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Volo.Abp.ObjectMapping;
using Volo.Abp.Reflection;
using Volo.Abp.Threading;
using Volo.Abp.Uow;
using Volo.Abp.Validation;
using Volo.Abp.VirtualFileSystem;

namespace Volo.Abp
{
    [DependsOn(typeof(AbpLocalizationModule))]
    [DependsOn(typeof(AbpVirtualFileSystemModule))]
    public class AbpCommonModule : AbpModule
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
            services.AddSingleton<ICancellationTokenProvider>(NullCancellationTokenProvider.Instance);
            services.AddSingleton<IRequestedApiVersion>(NullRequestedApiVersion.Instance);
            services.AddSingleton(typeof(IDataFilter<>), typeof(DataFilter<>));
            
            services.AddAssemblyOf<AbpCommonModule>();
        }
    }
}
