using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Volo.Abp.Http.Client;

public interface IRemoteServiceConfigurationProvider
{
    [ItemNotNull]
    Task<RemoteServiceConfiguration> GetConfigurationOrDefaultAsync(string name);

    Task<RemoteServiceConfiguration?> GetConfigurationOrDefaultOrNullAsync(string name);
}
