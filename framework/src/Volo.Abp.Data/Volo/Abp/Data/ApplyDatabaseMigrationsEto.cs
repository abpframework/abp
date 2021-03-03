using System;
using Volo.Abp.EventBus;

namespace Volo.Abp.Data
{
    [Serializable]
    [EventName("abp.data.apply_database_migrations")]
    public class ApplyDatabaseMigrationsEto
    {
        public Guid? TenantId { get; set; }
        
        public string DatabaseName { get; set; }
    }
}