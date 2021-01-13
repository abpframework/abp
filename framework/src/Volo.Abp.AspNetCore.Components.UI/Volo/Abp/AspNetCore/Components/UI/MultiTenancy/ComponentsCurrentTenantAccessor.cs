using Volo.Abp.DependencyInjection;
using Volo.Abp.MultiTenancy;

namespace Volo.Abp.AspNetCore.Components.UI.MultiTenancy
{
    [Dependency(ReplaceServices = true)]
    public class ComponentsCurrentTenantAccessor : ICurrentTenantAccessor, ISingletonDependency
    {
        public BasicTenantInfo Current { get; set; }
    }
}
