using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.EventBus.RabbitMq
{
    public class RabbitMqEventErrorHandler : EventErrorHandlerBase, ISingletonDependency
    {
        public const string HeadersKey = "headers";
        public const string RetryIndexKey = "retryIndex";

        public RabbitMqEventErrorHandler(
            IOptions<AbpEventBusOptions> options)
            : base(options)
        {
        }

        protected override async Task Retry(EventExecutionErrorContext context)
        {
            if (Options.RetryStrategyOptions.IntervalMillisecond > 0)
            {
                await Task.Delay(Options.RetryStrategyOptions.IntervalMillisecond);
            }

            var headers = context.GetProperty<Dictionary<string, object>>(HeadersKey) ??
                          new Dictionary<string, object>();

            var index = 1;
            if (headers.ContainsKey(RetryIndexKey))
            {
                index = (int) headers[RetryIndexKey];
                headers[RetryIndexKey] = ++index;
            }
            else
            {
                headers[RetryIndexKey] = index;
            }

            headers["exceptions"] = context.Exceptions;

            await context.EventBus.As<RabbitMqDistributedEventBus>().PublishAsync(context.EventType, context.EventData, headers);
        }

        protected override Task MoveToDeadLetter(EventExecutionErrorContext context)
        {
            if (context.Exceptions.Count == 1)
            {
                context.Exceptions[0].ReThrow();
            }

            throw new AggregateException(
                "More than one error has occurred while triggering the event: " + context.EventType,
                context.Exceptions);
        }

        protected override bool ShouldRetry(EventExecutionErrorContext context)
        {
            if (!base.ShouldRetry(context))
            {
                return false;
            }

            var headers = context.GetProperty<Dictionary<string, object>>(HeadersKey);

            if (headers == null || !headers.ContainsKey(RetryIndexKey))
            {
                return true;
            }

            var index = (int) headers[RetryIndexKey];

            return Options.RetryStrategyOptions.MaxRetryAttempts > index;
        }
    }
}
