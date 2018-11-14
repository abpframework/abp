using System;
using System.Linq;
using JetBrains.Annotations;

namespace Volo.Abp.EventBus
{
    [AttributeUsage(AttributeTargets.Class)]
    public class EventNameAttribute : Attribute, IEventNameProvider
    {
        public string Name { get; }

        public EventNameAttribute([NotNull] string name)
        {
            Name = Check.NotNullOrWhiteSpace(name, nameof(name));
        }

        public static string GetName<TEvent>()
        {
            return GetName(typeof(TEvent));
        }

        public static string GetName([NotNull] Type eventType)
        {
            Check.NotNull(eventType, nameof(eventType));

            return eventType
                       .GetCustomAttributes(true)
                       .OfType<IEventNameProvider>()
                       .FirstOrDefault()
                       ?.Name
                   ?? eventType.FullName;
        }
    }

    public interface IEventNameProvider
    {
        string Name { get; }
    }
}
