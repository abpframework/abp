using System;
using Volo.Abp.EventBus;

namespace Volo.Abp.Data;

[Serializable]
[EventName("abp.data.applied_database_migrations")]
public class AppliedDatabaseMigrationsEto
{
    public string DatabaseName { get; set; } = default!;
    public Guid? TenantId { get; set; }
}