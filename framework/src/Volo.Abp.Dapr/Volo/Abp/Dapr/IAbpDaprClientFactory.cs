using Dapr.Client;

namespace Volo.Abp.Dapr;

public interface IAbpDaprClientFactory
{
    Task<DaprClient> CreateAsync();
}