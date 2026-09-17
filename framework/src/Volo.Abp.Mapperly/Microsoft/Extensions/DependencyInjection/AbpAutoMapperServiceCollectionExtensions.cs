using Microsoft.Extensions.DependencyInjection.Extensions;
using Volo.Abp.Mapperly;
using Volo.Abp.ObjectMapping;

namespace Microsoft.Extensions.DependencyInjection;

public static class AbpAutoMapperServiceCollectionExtensions
{
    public static IServiceCollection AddMapperlyObjectMapper(this IServiceCollection services)
    {
        return services.Replace(
            ServiceDescriptor.Transient<IAutoObjectMappingProvider, MapperlyAutoObjectMappingProvider>()
        );
    }

    public static IServiceCollection AddMapperlyObjectMapper<TContext>(this IServiceCollection services)
    {
        return services.Replace(
            ServiceDescriptor.Transient<IAutoObjectMappingProvider<TContext>, MapperlyAutoObjectMappingProvider<TContext>>()
        );
    }
}
