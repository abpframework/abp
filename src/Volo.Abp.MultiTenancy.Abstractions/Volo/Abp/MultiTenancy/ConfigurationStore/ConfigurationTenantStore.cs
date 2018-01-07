using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.MultiTenancy.ConfigurationStore
{
    //TODO: Move to another package.
    [Dependency(TryRegister = true)]
    public class ConfigurationTenantStore : ITenantStore, ITransientDependency
    {
        private readonly ConfigurationTenantStoreOptions _options;

        public ConfigurationTenantStore(IOptionsSnapshot<ConfigurationTenantStoreOptions> options)
        {
            _options = options.Value;
        }

        public Task<TenantInfo> FindAsync(string name)
        {
            return Task.FromResult(_options.Tenants.FirstOrDefault(t => t.Name == name));
        }

        public Task<TenantInfo> FindAsync(Guid id)
        {
            return Task.FromResult(_options.Tenants.FirstOrDefault(t => t.Id == id));
        }
    }
}