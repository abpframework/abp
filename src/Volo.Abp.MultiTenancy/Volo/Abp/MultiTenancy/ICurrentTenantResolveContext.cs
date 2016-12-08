namespace Volo.Abp.MultiTenancy
{
    public interface ICurrentTenantResolveContext
    {
        TenantInfo Tenant { get; set; }

        bool Handled { get; set; }
    }
}