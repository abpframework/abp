using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Modularity;

namespace Volo.Abp.Uow
{
    public class AbpUnitOfWorkModule : AbpModule
    {
        public override void PreConfigureServices(IServiceCollection services)
        {
            services.OnRegistred(UnitOfWorkInterceptorRegistrar.RegisterIfNeeded);
        }

        public override void ConfigureServices(IServiceCollection services)
        {
            services.AddAssemblyOf<AbpUnitOfWorkModule>();
        }
    }
}
