using System;
using System.Linq;
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

        public TenantInfo Find(string name)
        {
            return _options.Tenants.FirstOrDefault(t => t.Name == name);
        }

        public TenantInfo Find(Guid id)
        {
            return _options.Tenants.FirstOrDefault(t => t.Id == id);
        }
    }
}