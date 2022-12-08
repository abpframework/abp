using Volo.Abp.Auditing;

namespace Volo.Abp.Domain.Entities.Events.Distributed.ExternalEntitySynchronizers.WithEntityVersion;

public class RemoteBookEto : EntityEto, IHasEntityVersion
{
    public int EntityVersion { get; set; }
    
    public int Sold { get; set; }
}