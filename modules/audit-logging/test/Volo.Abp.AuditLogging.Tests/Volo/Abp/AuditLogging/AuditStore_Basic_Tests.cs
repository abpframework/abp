using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Shouldly;
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
            //Arrange
            var userId = new Guid("4456fb0d-74cc-4807-9eee-23e551e6cb06");
            var ipAddress = "153.1.7.61";
            var firstComment = "first Comment";

            var auditLog = new AuditLogInfo
            {
                TenantId = Guid.NewGuid(),
                UserId = userId,
                ImpersonatorUserId = Guid.NewGuid(),
                ImpersonatorTenantId = Guid.NewGuid(),
                ExecutionTime = DateTime.Today,
                ExecutionDuration = 42,
                ClientIpAddress = ipAddress,
                ClientName = "MyDesktop",
                BrowserInfo = "Chrome",
                Comments = new List<string> { firstComment, "Second Comment"}
            };

            //Act
            await _auditingStore.SaveAsync(auditLog);

            //Assert

            var insertedLog = GetAuditLogsFromDbContext().FirstOrDefault(al => al.UserId == userId);

            insertedLog.ShouldNotBeNull();
            insertedLog.ClientIpAddress.ShouldBe(ipAddress);
            insertedLog.Comments.ShouldStartWith(firstComment);
        }
    }
}
