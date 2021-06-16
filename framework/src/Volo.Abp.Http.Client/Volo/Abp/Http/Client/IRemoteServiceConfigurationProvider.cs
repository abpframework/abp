using JetBrains.Annotations;

namespace Volo.Abp.Http.Client
{
    public interface IRemoteServiceConfigurationProvider
    {
        [NotNull]
        RemoteServiceConfiguration GetConfigurationOrDefault(string name);
        
        [CanBeNull]
        RemoteServiceConfiguration GetConfigurationOrDefaultOrNull(string name);
    }
}