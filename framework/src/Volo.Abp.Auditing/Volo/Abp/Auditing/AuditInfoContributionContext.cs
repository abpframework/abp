using System;

namespace Volo.Abp.Auditing
{
    public class AuditInfoContributionContext : IAuditInfoContributionContext
    {
        public IServiceProvider ServiceProvider { get; }

        public AuditInfo AuditInfo { get; set; }

        public AuditInfoContributionContext(IServiceProvider serviceProvider, AuditInfo auditInfo)
        {
            ServiceProvider = serviceProvider;
            AuditInfo = auditInfo;
        }
    }
}