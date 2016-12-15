namespace Volo.Abp.MultiTenancy
{
    public interface ICurrentTenantResolveContext : IServiceProviderAccessor
    {
        TenantInfo Tenant { get; set; }

        bool? Handled { get; set; }
    }
}