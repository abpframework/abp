using System;
using Volo.Abp.Domain.Entities;

namespace Volo.Abp.TestApp.Domain;

// Uses a custom bool<->int converter where false maps to a non-zero value, so the
// soft-delete DbFunction translator (which hardcodes a bool false literal with the
// bool TypeMapping) is observably wrong on every provider: the generated SQL
// compares against 0/FALSE instead of the converter's actual provider value (5).
public class EntityWithIntSoftDelete : AggregateRoot<Guid>, ISoftDelete
{
    public const string IsDeletedColumnName = "is_deleted";
    public const int NotDeletedProviderValue = 5;
    public const int DeletedProviderValue = 9;

    public string Name { get; set; }

    public bool IsDeleted { get; set; }
}
