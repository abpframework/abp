using Microsoft.Extensions.DependencyInjection.Extensions;
using Volo.Abp.Mapster;
using Volo.Abp.ObjectMapping;

namespace Microsoft.Extensions.DependencyInjection;

public static class AbpMapsterServiceCollectionExtensions
{
    public static IServiceCollection AddMapsterObjectMapper(this IServiceCollection services)
    {
        return services.Replace(ServiceDescriptor
            .Transient<IAutoObjectMappingProvider, MapsterAutoObjectMappingProvider>());
    }
    
    public static IServiceCollection AddMapsterObjectMapper<TContext>(this IServiceCollection services)
    {
        return services.Replace(
            ServiceDescriptor.Transient<IAutoObjectMappingProvider<TContext>, MapsterAutoObjectMappingProvider<TContext>>()
        );
    }
}