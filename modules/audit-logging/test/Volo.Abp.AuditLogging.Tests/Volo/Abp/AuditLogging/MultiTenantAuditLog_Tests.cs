using System;
using System.Linq;
using System.Threading.Tasks;
using Shouldly;
using Volo.Abp.Auditing;
using Xunit;

namespace Volo.Abp.AuditLogging
{
    public class MultiTenantAuditLog_Tests : AuditLogsTestBase
    {
        private readonly IAuditingManager _auditingManager;
        private readonly IAuditLogRepository _auditLogRepository;

        public MultiTenantAuditLog_Tests()
        {
            _auditingManager = GetRequiredService<IAuditingManager>();
            _auditLogRepository = GetRequiredService<IAuditLogRepository>();
        }

        [Fact]
        public async Task Should_Save_Audit_Logs_To_The_Tenant_Begins_The_Scope()
        {
            //Arrange

            var applicationName = Guid.NewGuid().ToString();
            var tenantId = Guid.NewGuid();
            var entityId1 = Guid.NewGuid();
            var entityId2 = Guid.NewGuid();

            //Act

            using (var scope = _auditingManager.BeginScope())
            {
                _auditingManager.Current.Log.ApplicationName = applicationName;

                //Creating a host entity
                _auditingManager.Current.Log.EntityChanges.Add(
                    new EntityChangeInfo
                    {
                        ChangeTime = DateTime.Now,
                        ChangeType = EntityChangeType.Created,
                        EntityEntry = new object(),
                        EntityId = entityId1.ToString(),
                        EntityTypeFullName = "TestEntity"
                    }
                );

                //Creating a tenant entity
                _auditingManager.Current.Log.EntityChanges.Add(
                    new EntityChangeInfo
                    {
                        ChangeTime = DateTime.Now,
                        ChangeType = EntityChangeType.Created,
                        EntityEntry = new object(),
                        EntityId = entityId2.ToString(),
                        EntityTypeFullName = "TestEntity",
                        EntityTenantId = tenantId
                    }
                );

                await scope.SaveAsync();
            }

            //Assert

            var auditLogs = await _auditLogRepository.GetListAsync(applicationName: applicationName, includeDetails: true);
            auditLogs.Count.ShouldBe(1);
            var auditLog = auditLogs.First();
            auditLog.EntityChanges.ShouldNotBeNull();
            auditLog.EntityChanges.Count.ShouldBe(2);
            auditLog.EntityChanges.ShouldContain(e => e.EntityId == entityId1.ToString());
            auditLog.EntityChanges.ShouldContain(e => e.EntityId == entityId2.ToString());
        }
    }
}
