using System;
using Volo.Abp.MultiTenancy;

namespace Volo.Abp.Identity
{
    public class SecurityLogEvent : IMultiTenant
    {
        public Guid? TenantId { get; set; }

        public string Identity { get; set; }

        public string Action { get; set; }

        public string UserName { get; set; }

        public string ClientId { get; set; }
    }
}
