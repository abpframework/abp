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
                UserId = userId,
                ImpersonatorUserId = Guid.NewGuid(),
                ImpersonatorTenantId = Guid.NewGuid(),
                ExecutionTime = DateTime.Today,
                ExecutionDuration = 42,
                ClientIpAddress = ipAddress,
                ClientName = "MyDesktop",
                BrowserInfo = "Chrome",
                Comments = new List<string> { firstComment, "Second Comment"},
                EntityChanges =
                {
                    new EntityChangeInfo
                    {
                        EntityId = Guid.NewGuid().ToString(),
                        EntityTypeFullName = typeof(AuditStore_Basic_Tests).FullName,
                        ChangeType = EntityChangeType.Created,
                        ChangeTime = DateTime.Now,
                        PropertyChanges = new List<EntityPropertyChangeInfo>
                        {
                            new EntityPropertyChangeInfo
                            {
                                PropertyTypeFullName = typeof(string).FullName,
                                PropertyName = "Name",
                                NewValue = "New value",
                                OriginalValue = null
                            }
                        }
                    }
                }
            };

            //Act
            await _auditingStore.SaveAsync(auditLog);

            //Assert

            var insertedLog = GetAuditLogsFromDbContext()
                .FirstOrDefault(al => al.UserId == userId);

            insertedLog.ShouldNotBeNull();
            insertedLog.ClientIpAddress.ShouldBe(ipAddress);
            insertedLog.Comments.ShouldStartWith(firstComment);
            insertedLog.EntityChanges.Count.ShouldBeGreaterThan(0);
            insertedLog.EntityChanges.First().PropertyChanges.Count.ShouldBeGreaterThan(0);
        }
    }
}
