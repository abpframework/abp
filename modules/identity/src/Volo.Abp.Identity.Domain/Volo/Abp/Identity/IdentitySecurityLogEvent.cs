using System;
using System.Collections.Generic;
using Volo.Abp.MultiTenancy;

namespace Volo.Abp.Identity
{
    public class IdentitySecurityLogEvent : IMultiTenant
    {
        public Guid? TenantId { get; set; }

        public string Identity { get; set; }

        public string Action { get; set; }

        public string UserName { get; set; }

        public string ClientId { get; set; }

        public Dictionary<string, object> ExtraProperties { get; }

        public IdentitySecurityLogEvent()
        {
            ExtraProperties = new Dictionary<string, object>();
        }

        public virtual IdentitySecurityLogEvent WithProperty(string key, object value)
        {
            ExtraProperties[key] = value;
            return this;
        }

    }
}
