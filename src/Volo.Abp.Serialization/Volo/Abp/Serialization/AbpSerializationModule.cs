using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Modularity;
using Volo.Abp.Reflection;

namespace Volo.Abp.Serialization
{
    public class AbpSerializationModule : AbpModule
    {
        public override void PreConfigureServices(IServiceCollection services)
        {
            services.OnExposing(context =>
            {
                //Register types for IObjectSerializer<T> if implements
                context.ExposedTypes.AddRange(
                    ReflectionHelper.GetImplementedGenericTypes(
                        context.ImplementationType,
                        typeof(IObjectSerializer<>)
                    )
                );
            });
        }

        public override void ConfigureServices(IServiceCollection services)
        {
            services.AddAssemblyOf<AbpSerializationModule>();
        }
    }
}
