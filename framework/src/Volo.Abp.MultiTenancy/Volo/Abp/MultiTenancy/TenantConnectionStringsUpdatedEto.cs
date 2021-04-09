using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities.Events.Distributed;
using Volo.Abp.EventBus;

namespace Volo.Abp.MultiTenancy
{
    [Serializable]
    [EventName("abp.multi_tenancy.tenant.connection_string.updated")]
    public class TenantConnectionStringsUpdatedEto : EtoBase
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public List<TenantConnectionStringEto> ConnectionStrings { get; set; }
    }
}
