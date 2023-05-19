namespace Volo.Abp.MultiTenancy;

/* A null Current indicates that we haven't set it explicitly.
 * A null Current.TenantId indicates that we have set null tenant id value explicitly.
 * A non-null Current.TenantId indicates that we have set a tenant id value explicitly.
 */

public interface ICurrentTenantAccessor
{
    BasicTenantInfo Current { get; set; }
}
