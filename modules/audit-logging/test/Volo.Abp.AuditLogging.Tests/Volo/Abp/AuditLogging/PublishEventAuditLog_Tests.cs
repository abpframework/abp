using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Shouldly;
using Volo.Abp.Auditing;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EventBus.Distributed;
using Xunit;

namespace Volo.Abp.AuditLogging;

public class PublishEventAuditLog_Tests : AuditLogsTestBase
{
    private readonly IAuditingStore _auditingStore;
    private readonly TestData _testData;

    public PublishEventAuditLog_Tests()
    {
        _testData = GetRequiredService<TestData>();
        _auditingStore = GetRequiredService<IAuditingStore>();
    }

    [Fact]
    public async Task Should_Handle_Published_Audit_Events()
    {
        // Arrange
        var userId = _testData.UserId;
        var userId2 = new Guid("4456fb0d-74cc-4807-9eee-23e551e6cb06");
        var ipAddress = "153.1.7.61";
        var firstComment = "first Comment";

        var log1 = new AuditLogInfo {
            UserId = userId,
            ImpersonatorUserId = Guid.NewGuid(),
            ImpersonatorTenantId = Guid.NewGuid(),
            ImpersonatorTenantName = "default",
            ImpersonatorUserName = "admin",
            ExecutionTime = DateTime.Today,
            ExecutionDuration = 42,
            ClientIpAddress = ipAddress,
            ClientName = "MyDesktop",
            BrowserInfo = "Chrome",
            Comments = new List<string> { firstComment, "Second Comment" },
            UserName = "Douglas",
            EntityChanges = {
                new EntityChangeInfo {
                    EntityId = Guid.NewGuid().ToString(),
                    EntityTypeFullName = "Volo.Abp.AuditLogging.TestEntity",
                    ChangeType = EntityChangeType.Created,
                    ChangeTime = DateTime.Now,
                    PropertyChanges = new List<EntityPropertyChangeInfo> {
                        new EntityPropertyChangeInfo {
                            PropertyTypeFullName = typeof(string).FullName,
                            PropertyName = "Name",
                            NewValue = "New value",
                            OriginalValue = null
                        }
                    }
                },
                new EntityChangeInfo {
                    EntityId = Guid.NewGuid().ToString(),
                    EntityTypeFullName = "Volo.Abp.AuditLogging.TestEntity",
                    ChangeType = EntityChangeType.Created,
                    ChangeTime = DateTime.Now,
                    PropertyChanges = new List<EntityPropertyChangeInfo> {
                        new EntityPropertyChangeInfo {
                            PropertyTypeFullName = typeof(string).FullName,
                            PropertyName = "Name",
                            NewValue = "New value",
                            OriginalValue = null
                        }
                    }
                }
            }
        };

        var log2 = new AuditLogInfo {
            UserId = userId2,
            ImpersonatorUserId = Guid.NewGuid(),
            ImpersonatorTenantId = Guid.NewGuid(),
            ImpersonatorTenantName = "default",
            ImpersonatorUserName = "admin",
            ExecutionTime = DateTime.Today,
            ExecutionDuration = 42,
            ClientIpAddress = ipAddress,
            ClientName = "MyDesktop",
            BrowserInfo = "Chrome",
            Comments = new List<string> { firstComment, "Second Comment" },
            HttpStatusCode = (int?)HttpStatusCode.BadGateway,
            EntityChanges = {
                new EntityChangeInfo {
                    EntityId = Guid.NewGuid().ToString(),
                    EntityTypeFullName = "Volo.Abp.AuditLogging.TestEntity",
                    ChangeType = EntityChangeType.Created,
                    ChangeTime = DateTime.Now,
                    PropertyChanges = new List<EntityPropertyChangeInfo> {
                        new EntityPropertyChangeInfo {
                            PropertyTypeFullName = typeof(string).FullName,
                            PropertyName = "Name",
                            NewValue = "New value",
                            OriginalValue = null
                        }
                    }
                }
            }
        };
        await _auditingStore.SaveAsync(log1);
        await _auditingStore.SaveAsync(log2);

        _testData.HandledAuditLogEventCount.ShouldBe(2);
    }

    [Fact]
    public async Task Should_Have_Correctly_Mapped_Audit_Data()
    {
        // Arrange
        var userId = _testData.UserId;
        var ipAddress = "153.1.7.61";
        var firstComment = "first Comment";

        var log1 = new AuditLogInfo {
            UserId = userId,
            ImpersonatorUserId = Guid.NewGuid(),
            ImpersonatorTenantId = Guid.NewGuid(),
            ImpersonatorTenantName = "default",
            ImpersonatorUserName = "admin",
            ExecutionTime = DateTime.Today,
            ExecutionDuration = 42,
            ClientIpAddress = ipAddress,
            ClientName = "MyDesktop",
            BrowserInfo = "Chrome",
            Comments = new List<string> { firstComment, "Second Comment" },
            UserName = "Douglas",
            EntityChanges = {
                new EntityChangeInfo {
                    EntityId = Guid.NewGuid().ToString(),
                    EntityTypeFullName = "Volo.Abp.AuditLogging.TestEntity",
                    ChangeType = EntityChangeType.Created,
                    ChangeTime = DateTime.Now,
                    PropertyChanges = new List<EntityPropertyChangeInfo> {
                        new EntityPropertyChangeInfo {
                            PropertyTypeFullName = typeof(string).FullName,
                            PropertyName = "Name",
                            NewValue = "New value",
                            OriginalValue = null
                        }
                    }
                },
                new EntityChangeInfo {
                    EntityId = Guid.NewGuid().ToString(),
                    EntityTypeFullName = "Volo.Abp.AuditLogging.TestEntity",
                    ChangeType = EntityChangeType.Created,
                    ChangeTime = DateTime.Now,
                    PropertyChanges = new List<EntityPropertyChangeInfo> {
                        new EntityPropertyChangeInfo {
                            PropertyTypeFullName = typeof(string).FullName,
                            PropertyName = "Name",
                            NewValue = "New value",
                            OriginalValue = null
                        }
                    }
                }
            }
        };

        await _auditingStore.SaveAsync(log1);
        _testData.HandledAuditLogEventCount.ShouldBe(1);
        _testData.UserLogInfo.UserId.ShouldBe(_testData.UserId);
        _testData.UserLogInfo.EntityChanges.ShouldContain(q => q.EntityTypeFullName == "Volo.Abp.AuditLogging.TestEntity");

        _testData.UserLogInfoEto.ClientIpAddress.ShouldBe(ipAddress);
        _testData.UserLogInfoEto.Comments.ShouldContain(q => q == firstComment);
    }
    
    [Fact]
    public async Task Should_Not_Publish_Event_When_Option_IsNot_Selected()
    {
        // Arrange
        var userId = _testData.UserId;
        var ipAddress = "153.1.7.61";
        var firstComment = "first Comment";

        var log1 = new AuditLogInfo {
            UserId = userId,
            ImpersonatorUserId = Guid.NewGuid(),
            ImpersonatorTenantId = Guid.NewGuid(),
            ImpersonatorTenantName = "default",
            ImpersonatorUserName = "admin",
            ExecutionTime = DateTime.Today,
            ExecutionDuration = 42,
            ClientIpAddress = ipAddress,
            ClientName = "MyDesktop",
            BrowserInfo = "Chrome",
            Comments = new List<string> { firstComment, "Second Comment" },
            UserName = "Douglas",
            EntityChanges = {
                new EntityChangeInfo {
                    EntityId = Guid.NewGuid().ToString(),
                    EntityTypeFullName = "Volo.Abp.AuditLogging.TestEntity",
                    ChangeType = EntityChangeType.Created,
                    ChangeTime = DateTime.Now,
                    PropertyChanges = new List<EntityPropertyChangeInfo> {
                        new EntityPropertyChangeInfo {
                            PropertyTypeFullName = typeof(string).FullName,
                            PropertyName = "Name",
                            NewValue = "New value",
                            OriginalValue = null
                        }
                    }
                },
                new EntityChangeInfo {
                    EntityId = Guid.NewGuid().ToString(),
                    EntityTypeFullName = "Volo.Abp.AuditLogging.TestEntity",
                    ChangeType = EntityChangeType.Created,
                    ChangeTime = DateTime.Now,
                    PropertyChanges = new List<EntityPropertyChangeInfo> {
                        new EntityPropertyChangeInfo {
                            PropertyTypeFullName = typeof(string).FullName,
                            PropertyName = "Name",
                            NewValue = "New value",
                            OriginalValue = null
                        }
                    }
                }
            }
        };

        using (var scope = ServiceProvider.CreateScope())
        {
            var options = scope.ServiceProvider.GetRequiredService<IOptions<AbpAuditingOptions>>().Value;
            options.PublishEvent = false;
            await _auditingStore.SaveAsync(log1);
            _testData.HandledAuditLogEventCount.ShouldBe(0);
        }
    }
}

public class MyAuditLoggingTestEventHandler : IDistributedEventHandler<AuditLogInfoEto>, ITransientDependency
{
    private readonly TestData _testData;
    private readonly IAuditLogEtoMapper _etoMapper;

    public MyAuditLoggingTestEventHandler(TestData testData, IAuditLogEtoMapper etoMapper)
    {
        _testData = testData;
        _etoMapper = etoMapper;
    }

    public async Task HandleEventAsync(AuditLogInfoEto eventData)
    {
        _testData.HandledAuditLogEventCount++;

        _testData.UserLogInfoEto = eventData;
        var auditLogInfo = await _etoMapper.MapToAuditLogInfoAsync(eventData);
        _testData.UserLogInfo = auditLogInfo;
    }
}