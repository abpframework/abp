namespace Volo.Abp.Modularity
{
    public class DefaultModuleLifecycleContributer : ModuleLifecycleContributerBase
    {
        public override void Initialize(ApplicationInitializationContext context, IAbpModule module)
        {
            (module as IOnApplicationInitialization)?.OnApplicationInitialization(context);
        }

        public override void Shutdown(ApplicationShutdownContext context, IAbpModule module)
        {
            (module as IOnApplicationShutdown)?.OnApplicationShutdown(context);
        }
    }
}