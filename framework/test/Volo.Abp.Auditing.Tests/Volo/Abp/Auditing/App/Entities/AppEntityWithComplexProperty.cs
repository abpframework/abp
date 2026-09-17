using System;
using Volo.Abp.Auditing;
using Volo.Abp.Domain.Entities.Auditing;

namespace Volo.Abp.Auditing.App.Entities;

public class AppEntityWithComplexProperty : FullAuditedAggregateRoot<Guid>
{
    public string Name { get; set; }

    public AppEntityContactInformation ContactInformation { get; set; }

    [DisableAuditing]
    public AppEntityContactInformation DisabledContactInformation { get; set; }

    public AppEntityWithComplexProperty()
    {
    }

    public AppEntityWithComplexProperty(Guid id, string name)
        : base(id)
    {
        Name = name;
    }
}

public class AppEntityContactInformation
{
    public string Street { get; set; } = string.Empty;

    public AppEntityContactLocation Location { get; set; } = new();
}

public class AppEntityContactLocation
{
    public string City { get; set; } = string.Empty;
}
