using System;

namespace Volo.Abp.DependencyInjection;

[ExposeServices(typeof(IRootServiceProvider))]
public class RootServiceProvider : IRootServiceProvider, ISingletonDependency
{
    protected IServiceProvider ServiceProvider { get; }
    
    public RootServiceProvider(IObjectAccessor<IServiceProvider> objectAccessor)
    {
        ServiceProvider = objectAccessor.Value;
    }

    public virtual object GetService(Type serviceType)
    {
        return ServiceProvider.GetService(serviceType);
    }
}