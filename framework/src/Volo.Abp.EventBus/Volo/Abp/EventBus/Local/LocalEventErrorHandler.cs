using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.EventBus.Local
{
    public class LocalEventErrorHandler : EventErrorHandlerBase, ISingletonDependency
    {
        protected Dictionary<Guid, int> RetryTracking { get; }

        public LocalEventErrorHandler(
            IOptions<AbpEventBusOptions> options)
            : base(options)
        {
            RetryTracking = new Dictionary<Guid, int>();
        }

        protected override async Task Retry(EventExecutionErrorContext context)
        {
            if (Options.RetryStrategyOptions.IntervalMillisecond > 0)
            {
                await Task.Delay(Options.RetryStrategyOptions.IntervalMillisecond);
            }

            var messageId = context.GetProperty<Guid>("messageId");

            await context.EventBus.As<LocalEventBus>().PublishAsync(new LocalEventMessage(messageId, context.EventData, context.EventType));

            RetryTracking.Remove(messageId);
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

            var messageId = context.GetProperty<Guid>("messageId");

            var index = RetryTracking.GetOrDefault(messageId);

            if (Options.RetryStrategyOptions.MaxRetryAttempts <= index)
            {
                RetryTracking.Remove(messageId);
                return false;
            }

            RetryTracking[messageId] = ++index;

            return true;
        }
    }
}
