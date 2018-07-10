using System;
using System.Threading.Tasks;
using Volo.Abp.Auditing;
using Xunit;

namespace Volo.Abp.AuditLogging
{
    public class AuditStore_Basic_Tests : AuditLogsTestBase
    {
        private readonly IAuditingStore _auditingStore;

        public AuditStore_Basic_Tests()
        {
            _auditingStore = GetRequiredService<IAuditingStore>();
        }

        [Fact]
        public async Task Should_Save_A_Audit_Log()
        {
            var auditLog = new AuditLogInfo()
            {
                TenantId = Guid.NewGuid(),
                UserId = Guid.NewGuid(),
                ImpersonatorUserId = Guid.NewGuid(),
                ImpersonatorTenantId = Guid.NewGuid(),
                ExecutionTime = DateTime.Today,
                ExecutionDuration = 42,
                ClientIpAddress = "153.1.7.61",
                ClientName = "MyDesktop",
                BrowserInfo = "Chrome"
            };

            await _auditingStore.SaveAsync(auditLog);
        }
    }
}
