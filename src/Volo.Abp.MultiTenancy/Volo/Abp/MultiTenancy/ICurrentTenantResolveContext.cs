namespace Volo.Abp.MultiTenancy
{
    public interface ICurrentTenantResolveContext
    {
        ITenantInfo Tenant { get; set; }

        bool Handled { get; set; }
    }
}