using System;

namespace Volo.Abp.Domain.Entities.Events.Distributed.EntitySynchronizers.WithoutEntityVersion;

public class RemoteAuthorEto : EntityEto<Guid>
{
    public string Name { get; set; }
}