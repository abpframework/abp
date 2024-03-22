using System;

namespace Volo.Abp.MultiQueue;

public class ReceiveEventData
{
    public Guid Id { get; set; }
    public string Name { get; set; }
}
