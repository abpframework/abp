using System;
using Volo.Abp.Domain.Entities;

namespace Volo.Abp.TestApp.Domain;

public class EntityWithCustomSoftDeleteColumn : AggregateRoot<Guid>, ISoftDelete
{
    public string Name { get; set; }

    public bool IsDeleted { get; set; }
}
