namespace Volo.Abp.Modularity
{
    public abstract class ModuleLifecycleContributerBase : IModuleLifecycleContributer
    {
        public virtual void Initialize(ApplicationInitializationContext context, IAbpModule module)
        {
        }

        public virtual void Shutdown(ApplicationShutdownContext context, IAbpModule module)
        {
        }
    }
}