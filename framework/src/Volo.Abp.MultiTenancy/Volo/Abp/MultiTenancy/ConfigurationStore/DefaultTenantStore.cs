using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.MultiTenancy.ConfigurationStore
{
    [Dependency(TryRegister = true)]
    public class DefaultTenantStore : ITenantStore, ITransientDependency
    {
        private readonly AbpDefaultTenantStoreOptions _options;

        public DefaultTenantStore(IOptionsMonitor<AbpDefaultTenantStoreOptions> options)
        {
            _options = options.CurrentValue;
        }

        public Task<TenantConfiguration> FindAsync(string name)
        {
            return Task.FromResult(Find(name));
        }

        public Task<TenantConfiguration> FindAsync(Guid id)
        {
            return Task.FromResult(Find(id));
        }

        public TenantConfiguration Find(string name)
        {
            return _options.Tenants?.FirstOrDefault(t => t.Name == name);
        }

        public TenantConfiguration Find(Guid id)
        {
            return _options.Tenants?.FirstOrDefault(t => t.Id == id);
        }
    }
}
