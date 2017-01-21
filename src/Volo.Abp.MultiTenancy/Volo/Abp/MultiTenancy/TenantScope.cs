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
        public TenantInformation Tenant { get; }
        
        public TenantScope([CanBeNull] TenantInformation tenant)
        {
            Tenant = tenant;
        }
    }
}