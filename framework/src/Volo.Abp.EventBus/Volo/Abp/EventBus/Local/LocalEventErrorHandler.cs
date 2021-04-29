using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.EventBus.Local
{
    [ExposeServices(typeof(LocalEventErrorHandler), typeof(IEventErrorHandler))]
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

            var messageId = context.GetProperty<Guid>(nameof(LocalEventMessage.MessageId));

            context.TryGetRetryAttempt(out var retryAttempt);
            RetryTracking[messageId] = ++retryAttempt;

            await context.EventBus.As<LocalEventBus>().PublishAsync(new LocalEventMessage(messageId, context.EventData, context.EventType));

            RetryTracking.Remove(messageId);
        }

        protected override Task MoveToDeadLetter(EventExecutionErrorContext context)
        {
            ThrowOriginalExceptions(context);

            return Task.CompletedTask;
        }

        protected override bool ShouldRetry(EventExecutionErrorContext context)
        {
            var messageId = context.GetProperty<Guid>(nameof(LocalEventMessage.MessageId));
            context.SetProperty(RetryAttemptKey, RetryTracking.GetOrDefault(messageId));

            if (base.ShouldRetry(context))
            {
                return true;
            }

            RetryTracking.Remove(messageId);
            return false;
        }
    }
}
