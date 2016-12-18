namespace Volo.Abp.MultiTenancy
{
    public interface ITenantResolver
    {
        void Resolve(ITenantResolveContext context);
    }
}