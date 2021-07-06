using Volo.Abp.Modularity;

namespace Volo.Abp.EventBus
{
    [DependsOn(typeof(AbpEventBusModule))]
    public class EventBusTestModule : AbpModule
    {
        public override void PreConfigureServices(ServiceConfigurationContext context)
        {
            PreConfigure<AbpEventBusOptions>(options =>
            {
                options.UseRetryStrategy(retryStrategyOptions =>
                {
                    retryStrategyOptions.IntervalMillisecond = 0;
                });

                options.ErrorHandleSelector = type => type == typeof(MyExceptionHandleEventData);
            });
        }
    }
}
