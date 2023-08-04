using JetBrains.Annotations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Volo.Abp;

namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionConfigurationExtensions
{
    public static IServiceCollection ReplaceConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        return services.Replace(ServiceDescriptor.Singleton<IConfiguration>(configuration));
    }

    [NotNull]
    public static IConfiguration GetConfiguration(this IServiceCollection services)
    {
        return services.GetConfigurationOrNull() ?? 
               throw new AbpException("Could not find an implementation of " + typeof(IConfiguration).AssemblyQualifiedName + " in the service collection.");
    }
    
    [CanBeNull]
    public static IConfiguration? GetConfigurationOrNull(this IServiceCollection services)
    {
        var hostBuilderContext = services.GetSingletonInstanceOrNull<HostBuilderContext>();
        if (hostBuilderContext?.Configuration != null)
        {
            return hostBuilderContext.Configuration as IConfigurationRoot;
        }

        return services.GetSingletonInstanceOrNull<IConfiguration>();
    }
}
