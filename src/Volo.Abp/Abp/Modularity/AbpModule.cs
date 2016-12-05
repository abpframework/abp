using Microsoft.Extensions.DependencyInjection;

namespace Volo.Abp.Modularity
{
    public abstract class AbpModule : IAbpModule, IOnApplicationInitialize
    {
        public virtual void ConfigureServices(IServiceCollection services)
        {
            
        }

        public virtual void OnApplicationInitialize()
        {
            
        }
    }
}