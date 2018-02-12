using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.Permissions
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
            return Task.FromResult(false);
        }

        public Task AddAsync(string name, string providerName, string providerKey)
        {
            Logger.LogWarning($"Setting the grant value for {name} is not possible because current permission store is {nameof(NullPermissionStore)}");
            return Task.CompletedTask;
        }

        public Task<List<string>> GetAllGrantedAsync(string providerName, string providerKey)
        {
            return Task.FromResult(new List<string>());
        }

        public Task RemoveAsync(string name, string providerName, string providerKey)
        {
            return Task.CompletedTask;
        }
    }
}