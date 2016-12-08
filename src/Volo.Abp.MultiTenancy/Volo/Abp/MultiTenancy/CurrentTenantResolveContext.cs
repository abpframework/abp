namespace Volo.Abp.MultiTenancy
{
    public class CurrentTenantResolveContext : ICurrentTenantResolveContext
    {
        public TenantInfo Tenant { get; set; }

        public bool Handled { get; set; }
    }
}