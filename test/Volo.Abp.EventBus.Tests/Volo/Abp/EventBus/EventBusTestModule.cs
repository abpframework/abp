using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Modularity;

namespace Volo.Abp.EventBus
{
    [DependsOn(typeof(AbpEventBusModule))]
    public class EventBusTestModule : AbpModule
    {
        public override void ConfigureServices(IServiceCollection services)
        {
            services.AddAssemblyOf<EventBusTestModule>();
        }
    }
}