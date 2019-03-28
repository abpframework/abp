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
        private readonly DefaultTenantStoreOptions _options;

        public DefaultTenantStore(IOptionsSnapshot<DefaultTenantStoreOptions> options)
        {
            _options = options.Value;
        }

        public Task<TenantConfiguration> FindAsync(string name)
        {
            return Task.FromResult(_options.Tenants?.FirstOrDefault(t => t.Name == name));
        }

        public Task<TenantConfiguration> FindAsync(Guid id)
        {
            return Task.FromResult(_options.Tenants?.FirstOrDefault(t => t.Id == id));
        }
    }
}