using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Modularity;
using Volo.Abp.Reflection;

namespace Volo.Abp.Serialization
{
    public class AbpSerializationModule : AbpModule
    {
        public override void PreConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.OnExposing(onServiceExposingContext =>
            {
                //Register types for IObjectSerializer<T> if implements
                onServiceExposingContext.ExposedTypes.AddRange(
                    ReflectionHelper.GetImplementedGenericTypes(
                        onServiceExposingContext.ImplementationType,
                        typeof(IObjectSerializer<>)
                    )
                );
            });
        }
    }
}
