using System;

namespace Volo.Abp.EventBus.Local;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
public class LocalEventHandlerOrderAttribute : Attribute
{
    /// <summary>
    /// Handlers execute in ascending numeric value of the Order property.
    /// </summary>
    public int Order { get; set; }

    public LocalEventHandlerOrderAttribute(int order)
    {
        Order = order;
    }
}
