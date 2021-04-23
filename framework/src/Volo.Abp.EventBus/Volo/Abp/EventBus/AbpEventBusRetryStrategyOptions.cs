namespace Volo.Abp.EventBus
{
    public class AbpEventBusRetryStrategyOptions
    {

        public int IntervalMillisecond { get; set; } = 3000;

        public int Count { get; set; } = 3;
    }
}
