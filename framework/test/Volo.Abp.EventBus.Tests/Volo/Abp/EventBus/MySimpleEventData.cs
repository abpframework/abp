using System;
using Volo.Abp.MultiTenancy;

namespace Volo.Abp.EventBus
{
    public class MySimpleEventData : IMultiTenant
    {
        public int Value { get; set; }

        public Guid? TenantId { get; }

        public MySimpleEventData(int value, Guid? tenantId = null)
        {
            Value = value;
            TenantId = tenantId;
        }
    }
}
