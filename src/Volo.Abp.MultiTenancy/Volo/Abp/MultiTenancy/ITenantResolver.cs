namespace Volo.Abp.MultiTenancy
{
    public interface ITenantResolver
    {
        void Resolve(ICurrentTenantResolveContext context);
    }
}