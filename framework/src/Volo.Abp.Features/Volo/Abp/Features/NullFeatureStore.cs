using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.Features
{
    [Dependency(TryRegister = true)]
    public class NullFeatureStore : IFeatureStore, ISingletonDependency
    {
        public ILogger<NullFeatureStore> Logger { get; set; }

        public NullFeatureStore()
        {
            Logger = NullLogger<NullFeatureStore>.Instance;
        }

        public Task<string> GetOrNullAsync(string name, string providerName, string providerKey)
        {
            return Task.FromResult((string) null);
        }
    }
}