using System;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.AspNetCore.Components.WebAssembly.DependencyInjection
{
    public class WebAssemblyClientScopeServiceProviderAccessor :
        IClientScopeServiceProviderAccessor,
        ISingletonDependency
    {
        public IServiceProvider ServiceProvider { get; set; }
    }
}
