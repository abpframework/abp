namespace Volo.Abp.Modularity
{
    public abstract class ModuleLifecycleContributerBase : IModuleLifecycleContributer
    {
        public virtual void Initialize(IAbpModule module)
        {
        }

        public virtual void Shutdown(IAbpModule module)
        {
        }
    }
}