using System;
using Volo.Abp.Auditing;

namespace Volo.Abp.Domain.Entities.Events.Distributed.ExternalEntitySynchronizers;

public class RemoteBookEto : EntityEto, IHasModificationTime
{
    public DateTime? LastModificationTime { get; set; }
    
    public int Sold { get; set; }
}