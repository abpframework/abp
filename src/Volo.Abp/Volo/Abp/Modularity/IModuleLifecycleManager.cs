namespace Volo.Abp.Modularity
{
    public interface IModuleLifecycleManager
    {
        void InitializeModules();

        void ShutdownModules();
    }
}