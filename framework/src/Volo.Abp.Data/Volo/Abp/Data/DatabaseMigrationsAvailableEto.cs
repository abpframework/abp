using System;
using Volo.Abp.EventBus;

namespace Volo.Abp.Data;

[Serializable]
[EventName("abp.data.database_migrations_available")]
public class DatabaseMigrationsAvailableEto
{
    public string DatabaseName { get; set; }
}