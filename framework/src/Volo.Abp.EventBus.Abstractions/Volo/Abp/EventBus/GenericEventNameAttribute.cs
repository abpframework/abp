using System;

namespace Volo.Abp.EventBus;

[AttributeUsage(AttributeTargets.Class)]
public class GenericEventNameAttribute : Attribute, IEventNameProvider
{
    public string Prefix { get; set; }

    public string Postfix { get; set; }

    public virtual string GetName(Type eventType)
    {
        if (!eventType.IsGenericType)
        {
            throw new AbpException($"Given type is not generic: {eventType.AssemblyQualifiedName}");
        }

        var genericArguments = eventType.GetGenericArguments();
        if (genericArguments.Length > 1)
        {
            throw new AbpException($"Given type has more than one generic argument: {eventType.AssemblyQualifiedName}");
        }

        var eventName = EventNameAttribute.GetNameOrDefault(genericArguments[0]);

        if (!Prefix.IsNullOrEmpty())
        {
            eventName = Prefix + eventName;
        }

        if (!Postfix.IsNullOrEmpty())
        {
            eventName = eventName + Postfix;
        }

        return eventName;
    }
}
