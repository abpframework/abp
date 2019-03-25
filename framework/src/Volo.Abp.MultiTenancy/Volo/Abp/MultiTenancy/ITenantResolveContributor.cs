namespace Volo.Abp.MultiTenancy
{
    public interface ITenantResolveContributor
    {
        void Resolve(ITenantResolveContext context);
    }
}