using System;

namespace Volo.Abp.MultiTenancy
{
    [Serializable]
    public class TenantConnectionStringEto
    {
        public string ConnectionStringName { get; set; }

        public string OldValue { get; set; }

        public string NewValue { get; set; }
    }
}
