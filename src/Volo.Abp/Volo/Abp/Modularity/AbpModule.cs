using Microsoft.Extensions.DependencyInjection;

namespace Volo.Abp.Modularity
{
    public abstract class AbpModule : IAbpModule, IOnApplicationInitialization
    {
        //TODO: Add a OnBeforeConfigureServices method.

        public virtual void ConfigureServices(IServiceCollection services)
        {
            
        }

        public virtual void OnApplicationInitialization(ApplicationInitializationContext context)
        {
            
        }
    }
}