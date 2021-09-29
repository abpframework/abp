using System.Collections.Generic;

namespace Volo.Abp.EntityFrameworkCore.DistributedEvents
{
    public class AbpEfCoreDistributedEventBusOptions
    {
        public Dictionary<string, ISqlAdapter> SqlAdapters { get; set; }

        public ISqlAdapter GetSqlAdapter(string connectionType)
        {
            return SqlAdapters.TryGetValue(connectionType, out var sqlAdapter) ? sqlAdapter : SqlAdapters[DefaultSqlAdapter.Name];
        }

        public AbpEfCoreDistributedEventBusOptions()
        {
            SqlAdapters = new Dictionary<string, ISqlAdapter>();
        }
    }
}
