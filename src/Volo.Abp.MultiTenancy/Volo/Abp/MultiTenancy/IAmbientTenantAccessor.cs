using JetBrains.Annotations;

namespace Volo.Abp.MultiTenancy
{
    public interface IAmbientTenantAccessor
    {
        AmbientTenantInfo AmbientTenant { get; set; }
    }

    public class AmbientTenantInfo
    {
        /// <summary>
        /// Null for host.
        /// </summary>
        public TenantInfo Tenant { get; set; }
        
        public AmbientTenantInfo([CanBeNull] TenantInfo tenant)
        {
            Tenant = tenant;
        }
    }
}