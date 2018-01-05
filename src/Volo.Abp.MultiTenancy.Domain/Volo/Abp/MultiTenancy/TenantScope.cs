using JetBrains.Annotations;

namespace Volo.Abp.MultiTenancy
{
    public class TenantScope
    {
        /// <summary>
        /// Null indicates the host.
        /// Not null value for a tenant.
        /// </summary>
        [CanBeNull]
        public Tenant Tenant { get; }
        
        public TenantScope([CanBeNull] Tenant tenant)
        {
            Tenant = tenant;
        }
    }
}