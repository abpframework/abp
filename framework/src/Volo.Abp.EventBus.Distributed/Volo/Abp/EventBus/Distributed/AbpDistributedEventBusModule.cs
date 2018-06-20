using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Modularity;

namespace Volo.Abp.EventBus.Distributed
{
    [DependsOn(typeof(AbpEventBusModule))]
    public class AbpDistributedEventBusModule : AbpModule
    {
        public override void ConfigureServices(IServiceCollection services)
        {
            services.AddAssemblyOf<AbpDistributedEventBusModule>();
        }
    }
}
