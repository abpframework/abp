using Microsoft.Extensions.DependencyInjection;

namespace Volo.Abp.Modularity
{
    public abstract class AbpModule : IAbpModule, IOnApplicationInitialization
    {
        public virtual void ConfigureServices(IServiceCollection services)
        {
            
        }

        public virtual void OnApplicationInitialization(ApplicationInitializationContext context)
        {
            
        }
    }
}