using JetBrains.Annotations;

namespace Volo.Abp.MultiTenancy
{
    public class AmbientTenantScope
    {
        /// <summary>
        /// Null indicates the host.
        /// Not null value for a tenant.
        /// </summary>
        public TenantInfo Tenant { get; set; }
        
        public AmbientTenantScope([CanBeNull] TenantInfo tenant)
        {
            Tenant = tenant;
        }
    }
}