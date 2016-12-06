namespace Volo.Abp.Modularity
{
    public interface IOnApplicationInitialization
    {
        void OnApplicationInitialization(ApplicationInitializationContext context);
    }
}