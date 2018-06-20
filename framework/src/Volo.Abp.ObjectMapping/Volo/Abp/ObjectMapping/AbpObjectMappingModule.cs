using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Modularity;
using Volo.Abp.Reflection;

namespace Volo.Abp.ObjectMapping
{
    public class AbpObjectMappingModule : AbpModule
    {
        public override void PreConfigureServices(IServiceCollection services)
        {
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
            services.AddAssemblyOf<AbpObjectMappingModule>();
        }
    }
}
