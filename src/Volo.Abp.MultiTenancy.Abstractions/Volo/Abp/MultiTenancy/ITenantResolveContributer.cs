namespace Volo.Abp.MultiTenancy
{
    public interface ITenantResolveContributer
    {
        void Resolve(ITenantResolveContext context);
    }
}