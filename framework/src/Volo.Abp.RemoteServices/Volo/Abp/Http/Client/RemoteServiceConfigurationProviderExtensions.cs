using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Volo.Abp.Http.Client;

public static class RemoteServiceConfigurationProviderExtensions
{
    [ItemNotNull]
    public static Task<RemoteServiceConfiguration> GetConfigurationOrDefaultAsync(
        this IRemoteServiceConfigurationProvider provider)
        => provider.GetConfigurationOrDefaultAsync(RemoteServiceConfigurationDictionary.DefaultName);
    
    public static Task<RemoteServiceConfiguration?> GetConfigurationOrDefaultOrNullAsync(
        this IRemoteServiceConfigurationProvider provider)
        => provider.GetConfigurationOrDefaultOrNullAsync(RemoteServiceConfigurationDictionary.DefaultName);
}