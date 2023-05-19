using System;
using Volo.Abp.Auditing;

namespace Volo.Abp.Domain.Entities.Events.Distributed.EntitySynchronizers.WithEntityVersion;

public class RemoteBookEto : EntityEto<Guid>, IHasEntityVersion
{
    public int EntityVersion { get; set; }
    
    public int Sold { get; set; }
}