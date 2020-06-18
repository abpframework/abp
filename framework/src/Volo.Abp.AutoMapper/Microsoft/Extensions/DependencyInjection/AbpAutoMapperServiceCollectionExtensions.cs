using Microsoft.Extensions.DependencyInjection.Extensions;
using Volo.Abp.AutoMapper;
using Volo.Abp.ObjectMapping;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class AbpAutoMapperServiceCollectionExtensions
    {
        public static IServiceCollection AddAutoMapperObjectMapper(this IServiceCollection services)
        {
            return services.Replace(
                ServiceDescriptor.Transient<IAutoObjectMappingProvider, AutoMapperAutoObjectMappingProvider>()
            );
        }

        public static IServiceCollection AddAutoMapperObjectMapper<TContext>(this IServiceCollection services)
        {
            return services.Replace(
                ServiceDescriptor.Transient<IAutoObjectMappingProvider<TContext>, AutoMapperAutoObjectMappingProvider<TContext>>()
            );
        }
    }
}
