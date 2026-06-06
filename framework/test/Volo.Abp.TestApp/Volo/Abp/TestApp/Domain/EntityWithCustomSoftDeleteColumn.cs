using System;
using Volo.Abp.Domain.Entities;

namespace Volo.Abp.TestApp.Domain;

public class EntityWithCustomSoftDeleteColumn : AggregateRoot<Guid>, ISoftDelete
{
    public const string IsDeletedColumnName = "custom_is_deleted_column";

    public string Name { get; set; }

    public bool IsDeleted { get; set; }
}
