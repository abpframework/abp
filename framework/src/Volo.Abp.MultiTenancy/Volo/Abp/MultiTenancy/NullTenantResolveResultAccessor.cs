using Volo.Abp.DependencyInjection;

namespace Volo.Abp.MultiTenancy
{
    public class NullTenantResolveResultAccessor : ITenantResolveResultAccessor, ISingletonDependency
    {
        public TenantResolveResult Result
        {
            get => null;
            set { }
        }
    }
}