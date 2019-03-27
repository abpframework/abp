namespace Volo.Abp.MultiTenancy
{
    public abstract class TenantResolveContributorBase : ITenantResolveContributor
    {
        public abstract string Name { get; }

        //TODO: We can make this async
        public abstract void Resolve(ITenantResolveContext context);
    }
}