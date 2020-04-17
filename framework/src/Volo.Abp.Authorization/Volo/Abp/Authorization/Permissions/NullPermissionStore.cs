using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Threading;

namespace Volo.Abp.Authorization.Permissions
{
    public class NullPermissionStore : IPermissionStore, ISingletonDependency
    {
        public ILogger<NullPermissionStore> Logger { get; set; }

        public NullPermissionStore()
        {
            Logger = NullLogger<NullPermissionStore>.Instance;
        }

        public Task<bool> IsGrantedAsync(string name, string providerName, string providerKey)
        {
            return TaskCache.FalseResult;
        }
    }
}