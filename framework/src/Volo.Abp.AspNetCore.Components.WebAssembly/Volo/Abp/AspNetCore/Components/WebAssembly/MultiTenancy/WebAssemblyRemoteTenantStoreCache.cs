using System;
using System.Collections.Generic;
using System.Linq;
using Volo.Abp.DependencyInjection;
using Volo.Abp.MultiTenancy;

namespace Volo.Abp.AspNetCore.Components.WebAssembly.MultiTenancy
{
    public class WebAssemblyRemoteTenantStoreCache : IScopedDependency
    {
        protected readonly List<TenantConfiguration> CachedTenants = new List<TenantConfiguration>();

        public virtual TenantConfiguration Find(string name)
        {
            return CachedTenants.FirstOrDefault(t => t.Name == name);
        }

        public virtual TenantConfiguration Find(Guid id)
        {
            return CachedTenants.FirstOrDefault(t => t.Id == id);
        }

        public virtual void Set(TenantConfiguration tenant)
        {
            var existingTenant = Find(tenant.Id);
            if (existingTenant != null)
            {
                existingTenant.Name = tenant.Name;
                return;
            }

            CachedTenants.Add(tenant);
        }
    }
}
