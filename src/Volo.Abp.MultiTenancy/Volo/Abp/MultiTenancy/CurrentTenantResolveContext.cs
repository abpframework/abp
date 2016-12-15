using System;

namespace Volo.Abp.MultiTenancy
{
    public class CurrentTenantResolveContext : ICurrentTenantResolveContext
    {
        public IServiceProvider ServiceProvider { get; }

        public TenantInfo Tenant { get; set; }

        public bool? Handled { get; set; }

        internal bool IsHandled()
        {
            return Handled == true || (Handled == null && Tenant != null);
        }

        public CurrentTenantResolveContext(IServiceProvider serviceProvider)
        {
            ServiceProvider = serviceProvider;
        }
    }
}