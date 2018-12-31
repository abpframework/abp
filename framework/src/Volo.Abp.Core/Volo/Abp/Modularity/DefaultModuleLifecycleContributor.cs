namespace Volo.Abp.Modularity
{
    public class OnApplicationInitializationModuleLifecycleContributor : ModuleLifecycleContributorBase
    {
        public override void Initialize(ApplicationInitializationContext context, IAbpModule module)
        {
            (module as IOnApplicationInitialization)?.OnApplicationInitialization(context);
        }
    }

    public class OnApplicationShutdownModuleLifecycleContributor : ModuleLifecycleContributorBase
    {
        public override void Shutdown(ApplicationShutdownContext context, IAbpModule module)
        {
            (module as IOnApplicationShutdown)?.OnApplicationShutdown(context);
        }
    }

    public class OnPreApplicationInitializationModuleLifecycleContributor : ModuleLifecycleContributorBase
    {
        public override void Initialize(ApplicationInitializationContext context, IAbpModule module)
        {
            (module as IOnPreApplicationInitialization)?.OnPreApplicationInitialization(context);
        }
    }

    public class OnPostApplicationInitializationModuleLifecycleContributor : ModuleLifecycleContributorBase
    {
        public override void Initialize(ApplicationInitializationContext context, IAbpModule module)
        {
            (module as IOnPostApplicationInitialization)?.OnPostApplicationInitialization(context);
        }
    }
}