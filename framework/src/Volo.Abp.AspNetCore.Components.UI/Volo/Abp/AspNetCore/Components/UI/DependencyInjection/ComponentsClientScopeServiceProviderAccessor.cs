using System;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.AspNetCore.Components.UI.DependencyInjection
{
    public class ComponentsClientScopeServiceProviderAccessor :
        IClientScopeServiceProviderAccessor,
        ISingletonDependency
    {
        public IServiceProvider ServiceProvider { get; set; }
    }
}
