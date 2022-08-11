using System;

namespace Volo.Abp.DependencyInjection;

[ExposeServices(typeof(IRootServiceProviderAccessor))]
public class RootServiceProviderAccessor : IRootServiceProviderAccessor, ISingletonDependency
{
    public IServiceProvider ServiceProvider { get; }
    
    public RootServiceProviderAccessor(IObjectAccessor<IServiceProvider> objectAccessor)
    {
        ServiceProvider = objectAccessor.Value;
    }
}