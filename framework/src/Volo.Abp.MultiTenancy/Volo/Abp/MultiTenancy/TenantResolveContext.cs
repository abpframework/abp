using System;

namespace Volo.Abp.MultiTenancy
{
    public class TenantResolveContext : ITenantResolveContext
    {
        public IServiceProvider ServiceProvider { get; }

        public string TenantIdOrName { get; set; }

        public bool Handled { get; set; }

        public bool HasResolvedTenantOrHost()
        {
            return Handled || TenantIdOrName != null;
        }

        public TenantResolveContext(IServiceProvider serviceProvider)
        {
            ServiceProvider = serviceProvider;
        }
    }
}