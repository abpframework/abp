using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Volo.Abp.Configuration;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionConfigurationExtensions
    {
        [Obsolete]
        public static IServiceCollection SetConfiguration(this IServiceCollection services, IConfigurationRoot configurationRoot)
        {
            return services.Replace(ServiceDescriptor.Singleton<IConfigurationAccessor>(new DefaultConfigurationAccessor(configurationRoot)));
        }

        public static IConfigurationRoot GetConfiguration(this IServiceCollection services)
        {
            var hostBuilderContext = services.GetSingletonInstanceOrNull<HostBuilderContext>();
            if (hostBuilderContext?.Configuration != null)
            {
                return hostBuilderContext.Configuration as IConfigurationRoot;
            }

            return services.GetSingletonInstance<IConfigurationAccessor>().Configuration;
        }
    }
}
