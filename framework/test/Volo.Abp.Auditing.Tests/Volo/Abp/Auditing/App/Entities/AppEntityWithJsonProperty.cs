using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities.Auditing;

namespace Volo.Abp.Auditing.App.Entities;

public class AppEntityWithJsonProperty : FullAuditedAggregateRoot<Guid>
{
    public string Name { get; set; }

    public JsonPropertyObject Data { get; set; }

    public int Count { get; set; }

    public AppEntityWithJsonProperty()
    {
    }

    public AppEntityWithJsonProperty(Guid id, string name) : base(id)
    {
        Name = name;
    }
}

public class JsonPropertyObject : Dictionary<string, object>
{
}
