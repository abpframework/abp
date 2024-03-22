using System;

namespace Volo.Abp.MultiQueue.Options;

public class QueueOptionsTypeAttribute : Attribute
{
    public QueueOptionsTypeAttribute(string type)
    {
        Type = type;
    }
    public string Type { get; set; }
}
