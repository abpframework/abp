using System;

namespace Volo.Abp.MultiTenancy
{
    public class TenantResolveContext : ITenantResolveContext
    {
        public IServiceProvider ServiceProvider { get; }

        public TenantInfo Tenant { get; set; }

        public bool? Handled { get; set; }

        internal bool IsHandled()
        {
            return Handled == true || (Handled == null && Tenant != null);
        }

        public TenantResolveContext(IServiceProvider serviceProvider)
        {
            ServiceProvider = serviceProvider;
        }
    }
}