namespace Volo.Abp.MultiTenancy
{
    /* Uses TenantScopeTenantInfoWrapper instead of TenantInfo because being null of Current is different that being null of Current.Tenant.
    * A null Current indicates that we haven't set it explicitly.
    * A null Current.Tenant indicates that we have set null tenant value explicitly.
    * A non-null Current.Tenant indicates that we have set a tenant value explicitly.
    */

    public interface ICurrentTenantIdAccessor
    {
        BasicTenantInfo Current { get; set; }
    }
}