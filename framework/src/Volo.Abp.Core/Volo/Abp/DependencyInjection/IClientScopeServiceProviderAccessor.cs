using System;

namespace Volo.Abp.DependencyInjection
{
    public interface IClientScopeServiceProviderAccessor
    {
        IServiceProvider ServiceProvider { get; }
    }
}
