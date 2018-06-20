using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Volo.Abp.Configuration;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionConfigurationExtensions
    {
        public static IServiceCollection AddConfiguration(this IServiceCollection services, IConfigurationRoot configurationRoot)
        {
            return services.Replace(ServiceDescriptor.Singleton<IConfigurationAccessor>(new DefaultConfigurationAccessor(configurationRoot)));
        }

        public static IConfigurationRoot GetConfiguration(this IServiceCollection services)
        {
            return services.GetSingletonInstance<IConfigurationAccessor>().Configuration;
        }
    }
}
