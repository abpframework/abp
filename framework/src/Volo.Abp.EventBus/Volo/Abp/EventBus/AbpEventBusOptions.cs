using System;

namespace Volo.Abp.EventBus
{
    public class AbpEventBusOptions
    {
        public bool EnabledErrorHandle { get; set; }

        public Func<Type, bool> ErrorHandleSelector { get; set; }

        public string DeadLetterName { get; set; }

        public AbpEventBusRetryStrategyOptions RetryStrategyOptions { get; set; }

        public void UseRetryStrategy(Action<AbpEventBusRetryStrategyOptions> action = null)
        {
            RetryStrategyOptions = new AbpEventBusRetryStrategyOptions();
            action?.Invoke(RetryStrategyOptions);
        }
    }
}
