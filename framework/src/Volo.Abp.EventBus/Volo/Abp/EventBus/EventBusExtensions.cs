using System;
using JetBrains.Annotations;
using Volo.Abp.Threading;

namespace Volo.Abp.EventBus
{
    public static class EventBusExtensions
    {
        /// <summary>
        /// Triggers an event.
        /// </summary>
        /// <typeparam name="TEvent">Event type</typeparam>
        /// <param name="eventBus">Event bus instance</param>
        /// <param name="eventData">Related data for the event</param>
        public static void Publish<TEvent>([NotNull] this IEventBus eventBus, [NotNull] TEvent eventData)
            where TEvent : class
        {
            Check.NotNull(eventBus, nameof(eventBus));

            AsyncHelper.RunSync(() => eventBus.PublishAsync(eventData));
        }

        /// <summary>
        /// Triggers an event.
        /// </summary>
        /// <param name="eventBus">Event bus instance</param>
        /// <param name="eventType">Event type</param>
        /// <param name="eventData">Related data for the event</param>
        public static void Publish([NotNull] this IEventBus eventBus, [NotNull] Type eventType, [NotNull] object eventData)
        {
            Check.NotNull(eventBus, nameof(eventBus));

            AsyncHelper.RunSync(() => eventBus.PublishAsync(eventType, eventData));
        }
    }
}
