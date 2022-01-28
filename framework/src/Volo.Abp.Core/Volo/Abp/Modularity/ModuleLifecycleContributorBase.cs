using System.Threading.Tasks;

namespace Volo.Abp.Modularity;

public abstract class ModuleLifecycleContributorBase : IModuleLifecycleContributor
{
    public virtual Task InitializeAsync(ApplicationInitializationContext context, IAbpModule module)
    {
        return Task.CompletedTask;
    }

    public virtual void Initialize(ApplicationInitializationContext context, IAbpModule module)
    {
    }

    public virtual Task ShutdownAsync(ApplicationShutdownContext context, IAbpModule module)
    {
        return Task.CompletedTask;
    }

    public virtual void Shutdown(ApplicationShutdownContext context, IAbpModule module)
    {
    }
}
