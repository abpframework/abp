namespace Volo.Abp.MultiTenancy
{
    public class CurrentTenantResolveContext : ICurrentTenantResolveContext
    {
        public ITenantInfo Tenant { get; set; }

        public bool Handled { get; set; }
    }
}