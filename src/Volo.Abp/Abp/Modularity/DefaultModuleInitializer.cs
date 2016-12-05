namespace Volo.Abp.Modularity
{
    public class DefaultModuleInitializer : IModuleInitializer
    {
        public void Initialize(IAbpModule module)
        {
            (module as IOnApplicationInitialize)?.OnApplicationInitialize();
        }
    }
}