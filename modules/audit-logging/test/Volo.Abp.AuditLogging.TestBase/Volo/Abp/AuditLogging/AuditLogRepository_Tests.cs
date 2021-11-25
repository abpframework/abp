using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Shouldly;
using Volo.Abp.Auditing;
using Volo.Abp.Modularity;
using Xunit;

namespace Volo.Abp.AuditLogging;

public abstract class AuditLogRepository_Tests<TStartupModule> : AuditLoggingTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{
    protected IAuditLogRepository AuditLogRepository { get; }
    protected IAuditLogInfoToAuditLogConverter AuditLogInfoToAuditLogConverter { get; }

    protected AuditLogRepository_Tests()
    {
        AuditLogRepository = GetRequiredService<IAuditLogRepository>();
        AuditLogInfoToAuditLogConverter = GetRequiredService<IAuditLogInfoToAuditLogConverter>();
    }

    [Fact]
    public async Task GetListAsync()
    {
        // Arrange
        var userId = new Guid("4456fb0d-74cc-4807-9eee-23e551e6cb06");
        var userId2 = new Guid("4456fb0d-74cc-4807-9eee-23e551e6cb06");
        var ipAddress = "153.1.7.61";
        var firstComment = "first Comment";

        var log1 = new AuditLogInfo
        {
            UserId = userId,
            ImpersonatorUserId = Guid.NewGuid(),
            ImpersonatorTenantId = Guid.NewGuid(),
            ExecutionTime = DateTime.Today,
            ExecutionDuration = 42,
            ClientIpAddress = ipAddress,
            ClientName = "MyDesktop",
            BrowserInfo = "Chrome",
            Comments = new List<string> { firstComment, "Second Comment" },
            UserName = "Douglas",
            EntityChanges = {
                    new EntityChangeInfo
                {
                    EntityId = Guid.NewGuid().ToString(),
                    EntityTypeFullName = "Volo.Abp.AuditLogging.TestEntity",
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
                },
                    new EntityChangeInfo
                {
                    EntityId = Guid.NewGuid().ToString(),
                    EntityTypeFullName = "Volo.Abp.AuditLogging.TestEntity",
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

        var log2 = new AuditLogInfo
        {
            UserId = userId2,
            ImpersonatorUserId = Guid.NewGuid(),
            ImpersonatorTenantId = Guid.NewGuid(),
            ExecutionTime = DateTime.Today,
            ExecutionDuration = 42,
            ClientIpAddress = ipAddress,
            ClientName = "MyDesktop",
            BrowserInfo = "Chrome",
            Comments = new List<string> { firstComment, "Second Comment" },
            HttpStatusCode = (int?)HttpStatusCode.BadGateway,
            EntityChanges = {
                    new EntityChangeInfo
                    {
                        EntityId = Guid.NewGuid().ToString(),
                        EntityTypeFullName = "Volo.Abp.AuditLogging.TestEntity",
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

        await AuditLogRepository.InsertAsync(await AuditLogInfoToAuditLogConverter.ConvertAsync(log1));
        await AuditLogRepository.InsertAsync(await AuditLogInfoToAuditLogConverter.ConvertAsync(log2));

        //Assert
        var logs = await AuditLogRepository.GetListAsync();
        logs.ShouldNotBeNull();
        logs.ShouldContain(x => x.UserId == userId);
        logs.ShouldContain(x => x.UserId == userId2);
    }

    [Fact]
    public async Task GetCountAsync()
    {
        // Arrange
        var userId = new Guid("4456fb0d-74cc-4807-9eee-23e551e6cb06");
        var userId2 = new Guid("4456fb0d-74cc-4807-9eee-23e551e6cb06");
        var ipAddress = "153.1.7.61";
        var firstComment = "first Comment";

        var log1 = new AuditLogInfo
        {
            UserId = userId,
            ImpersonatorUserId = Guid.NewGuid(),
            ImpersonatorTenantId = Guid.NewGuid(),
            ExecutionTime = DateTime.Today,
            ExecutionDuration = 42,
            ClientIpAddress = ipAddress,
            ClientName = "MyDesktop",
            BrowserInfo = "Chrome",
            Comments = new List<string> { firstComment, "Second Comment" },
            UserName = "Douglas",
            EntityChanges = {
                    new EntityChangeInfo
                {
                    EntityId = Guid.NewGuid().ToString(),
                    EntityTypeFullName = "Volo.Abp.AuditLogging.TestEntity",
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
                },
                    new EntityChangeInfo
                {
                    EntityId = Guid.NewGuid().ToString(),
                    EntityTypeFullName = "Volo.Abp.AuditLogging.TestEntity",
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

        var log2 = new AuditLogInfo
        {
            UserId = userId2,
            ImpersonatorUserId = Guid.NewGuid(),
            ImpersonatorTenantId = Guid.NewGuid(),
            ExecutionTime = DateTime.Today,
            ExecutionDuration = 42,
            ClientIpAddress = ipAddress,
            ClientName = "MyDesktop",
            BrowserInfo = "Chrome",
            Comments = new List<string> { firstComment, "Second Comment" },
            HttpStatusCode = (int?)HttpStatusCode.BadGateway,
            EntityChanges = {
                    new EntityChangeInfo
                    {
                        EntityId = Guid.NewGuid().ToString(),
                        EntityTypeFullName = "Volo.Abp.AuditLogging.TestEntity",
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

        await AuditLogRepository.InsertAsync(await AuditLogInfoToAuditLogConverter.ConvertAsync(log1));
        await AuditLogRepository.InsertAsync(await AuditLogInfoToAuditLogConverter.ConvertAsync(log2));

        //Assert
        var logs = await AuditLogRepository.GetCountAsync();
        logs.ShouldBe(2);
    }

    [Fact]
    public async Task GetAverageExecutionDurationPerDayAsync()
    {
        // Arrange
        var userId = new Guid("4456fb0d-74cc-4807-9eee-23e551e6cb06");
        var userId2 = new Guid("4456fb0d-74cc-4807-9eee-23e551e6cb06");
        var ipAddress = "153.1.7.61";
        var firstComment = "first Comment";

        var log1 = new AuditLogInfo
        {
            UserId = userId,
            ImpersonatorUserId = Guid.NewGuid(),
            ImpersonatorTenantId = Guid.NewGuid(),
            ExecutionTime = DateTime.SpecifyKind(DateTime.Parse("2020-01-01 01:00:00"), DateTimeKind.Utc),
            ExecutionDuration = 45,
            ClientIpAddress = ipAddress,
            ClientName = "MyDesktop",
            BrowserInfo = "Chrome",
            Comments = new List<string> { firstComment, "Second Comment" },
            UserName = "Douglas",
            EntityChanges = {
                    new EntityChangeInfo
                {
                    EntityId = Guid.NewGuid().ToString(),
                    EntityTypeFullName = "Volo.Abp.AuditLogging.TestEntity",
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
                },
                    new EntityChangeInfo
                {
                    EntityId = Guid.NewGuid().ToString(),
                    EntityTypeFullName = "Volo.Abp.AuditLogging.TestEntity",
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

        var log2 = new AuditLogInfo
        {
            UserId = userId2,
            ImpersonatorUserId = Guid.NewGuid(),
            ImpersonatorTenantId = Guid.NewGuid(),
            ExecutionTime = DateTime.SpecifyKind(DateTime.Parse("2020-01-01 03:00:00"), DateTimeKind.Utc),
            ExecutionDuration = 55,
            ClientIpAddress = ipAddress,
            ClientName = "MyDesktop",
            BrowserInfo = "Chrome",
            Comments = new List<string> { firstComment, "Second Comment" },
            HttpStatusCode = (int?)HttpStatusCode.BadGateway,
            EntityChanges = {
                    new EntityChangeInfo
                    {
                        EntityId = Guid.NewGuid().ToString(),
                        EntityTypeFullName = "Volo.Abp.AuditLogging.TestEntity",
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

        await AuditLogRepository.InsertAsync(await AuditLogInfoToAuditLogConverter.ConvertAsync(log1));
        await AuditLogRepository.InsertAsync(await AuditLogInfoToAuditLogConverter.ConvertAsync(log2));

        //Assert
        var date = DateTime.Parse("2020-01-01");
        var results = await AuditLogRepository.GetAverageExecutionDurationPerDayAsync(date, date);
        results.Count.ShouldBe(1);
        results.Values.First().ShouldBe(50); // (45 + 55) / 2
    }

    [Fact]
    public async Task GetEntityChangeListAsync()
    {
        // Arrange
        var userId = new Guid("4456fb0d-74cc-4807-9eee-23e551e6cb06");
        var userId2 = new Guid("4456fb0d-74cc-4807-9eee-23e551e6cb06");
        var ipAddress = "153.1.7.61";
        var firstComment = "first Comment";

        var log1 = new AuditLogInfo
        {
            UserId = userId,
            ImpersonatorUserId = Guid.NewGuid(),
            ImpersonatorTenantId = Guid.NewGuid(),
            ExecutionTime = DateTime.Today,
            ExecutionDuration = 42,
            ClientIpAddress = ipAddress,
            ClientName = "MyDesktop",
            BrowserInfo = "Chrome",
            Comments = new List<string> { firstComment, "Second Comment" },
            UserName = "Douglas",
            EntityChanges = {
                    new EntityChangeInfo
                {
                    EntityId = Guid.NewGuid().ToString(),
                    EntityTypeFullName = "Volo.Abp.AuditLogging.TestEntity_Deleted",
                    ChangeType = EntityChangeType.Deleted,
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
                },
                    new EntityChangeInfo
                {
                    EntityId = Guid.NewGuid().ToString(),
                    EntityTypeFullName = "Volo.Abp.AuditLogging.TestEntity_Created",
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

        var log2 = new AuditLogInfo
        {
            UserId = userId2,
            ImpersonatorUserId = Guid.NewGuid(),
            ImpersonatorTenantId = Guid.NewGuid(),
            ExecutionTime = DateTime.Today,
            ExecutionDuration = 42,
            ClientIpAddress = ipAddress,
            ClientName = "MyDesktop",
            BrowserInfo = "Chrome",
            Comments = new List<string> { firstComment, "Second Comment" },
            HttpStatusCode = (int?)HttpStatusCode.BadGateway,
            EntityChanges = {
                    new EntityChangeInfo
                    {
                        EntityId = Guid.NewGuid().ToString(),
                        EntityTypeFullName = "Volo.Abp.AuditLogging.TestEntity_Updated",
                        ChangeType = EntityChangeType.Updated,
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

        await AuditLogRepository.InsertAsync(await AuditLogInfoToAuditLogConverter.ConvertAsync(log1));
        await AuditLogRepository.InsertAsync(await AuditLogInfoToAuditLogConverter.ConvertAsync(log2));

        //Assert
        var entityChanges = await AuditLogRepository.GetEntityChangeListAsync();
        entityChanges.ShouldNotBeNull();
        entityChanges.Count.ShouldBe(3);

        entityChanges.Single(x => x.ChangeType == EntityChangeType.Created).ShouldNotBeNull();
        entityChanges.Single(x => x.ChangeType == EntityChangeType.Deleted).ShouldNotBeNull();
        entityChanges.Single(x => x.ChangeType == EntityChangeType.Updated).ShouldNotBeNull();
    }

    [Fact]
    public async Task GetEntityChangeAsync()
    {
        // Arrange
        var userId = new Guid("4456fb0d-74cc-4807-9eee-23e551e6cb06");
        var userId2 = new Guid("4456fb0d-74cc-4807-9eee-23e551e6cb06");
        var ipAddress = "153.1.7.61";
        var firstComment = "first Comment";

        var log1 = new AuditLogInfo
        {
            UserId = userId,
            ImpersonatorUserId = Guid.NewGuid(),
            ImpersonatorTenantId = Guid.NewGuid(),
            ExecutionTime = DateTime.Today,
            ExecutionDuration = 42,
            ClientIpAddress = ipAddress,
            ClientName = "MyDesktop",
            BrowserInfo = "Chrome",
            Comments = new List<string> { firstComment, "Second Comment" },
            UserName = "Douglas",
            EntityChanges = {
                    new EntityChangeInfo
                {
                    EntityId = Guid.NewGuid().ToString(),
                    EntityTypeFullName = "Volo.Abp.AuditLogging.TestEntity_Deleted",
                    ChangeType = EntityChangeType.Deleted,
                    ChangeTime = new DateTime(1995, 3, 27),
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
                },
                    new EntityChangeInfo
                {
                    EntityId = Guid.NewGuid().ToString(),
                    EntityTypeFullName = "Volo.Abp.AuditLogging.TestEntity_Created",
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

        var log2 = new AuditLogInfo
        {
            UserId = userId2,
            ImpersonatorUserId = Guid.NewGuid(),
            ImpersonatorTenantId = Guid.NewGuid(),
            ExecutionTime = DateTime.Today,
            ExecutionDuration = 42,
            ClientIpAddress = ipAddress,
            ClientName = "MyDesktop",
            BrowserInfo = "Chrome",
            Comments = new List<string> { firstComment, "Second Comment" },
            HttpStatusCode = (int?)HttpStatusCode.BadGateway,
            EntityChanges = {
                    new EntityChangeInfo
                    {
                        EntityId = Guid.NewGuid().ToString(),
                        EntityTypeFullName = "Volo.Abp.AuditLogging.TestEntity_Updated",
                        ChangeType = EntityChangeType.Updated,
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

        await AuditLogRepository.InsertAsync(await AuditLogInfoToAuditLogConverter.ConvertAsync(log1));
        await AuditLogRepository.InsertAsync(await AuditLogInfoToAuditLogConverter.ConvertAsync(log2));

        var entityChanges = await AuditLogRepository.GetEntityChangeListAsync();
        var entityChange =
            await AuditLogRepository.GetEntityChange(entityChanges.First().Id);

        entityChange.ChangeTime.ShouldBe(entityChanges.First().ChangeTime);
    }

    [Fact]
    public async Task GetOrderedEntityChangeListAsync()
    {
        // Arrange
        var userId = new Guid("4456fb0d-74cc-4807-9eee-23e551e6cb06");
        var userId2 = new Guid("4456fb0d-74cc-4807-9eee-23e551e6cb06");
        var ipAddress = "153.1.7.61";
        var firstComment = "first Comment";

        var deletedEntityChangeTime = new DateTime(2000, 05, 05, 05, 05, 05);
        var createdEntityChangeTime = new DateTime(2005, 05, 05, 05, 05, 05);
        var updatedEntityChangeTime = new DateTime(2010, 05, 05, 05, 05, 05);

        var log1 = new AuditLogInfo
        {
            UserId = userId,
            ImpersonatorUserId = Guid.NewGuid(),
            ImpersonatorTenantId = Guid.NewGuid(),
            ExecutionTime = DateTime.Today,
            ExecutionDuration = 42,
            ClientIpAddress = ipAddress,
            ClientName = "MyDesktop",
            BrowserInfo = "Chrome",
            Comments = new List<string> { firstComment, "Second Comment" },
            UserName = "Douglas",
            EntityChanges = {
                    new EntityChangeInfo
                {
                    EntityId = Guid.NewGuid().ToString(),
                    EntityTypeFullName = "Volo.Abp.AuditLogging.TestEntity_Deleted",
                    ChangeType = EntityChangeType.Deleted,
                    ChangeTime = deletedEntityChangeTime,
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
                },
                    new EntityChangeInfo
                {
                    EntityId = Guid.NewGuid().ToString(),
                    EntityTypeFullName = "Volo.Abp.AuditLogging.TestEntity_Created",
                    ChangeType = EntityChangeType.Created,
                    ChangeTime = createdEntityChangeTime,
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

        var log2 = new AuditLogInfo
        {
            UserId = userId2,
            ImpersonatorUserId = Guid.NewGuid(),
            ImpersonatorTenantId = Guid.NewGuid(),
            ExecutionTime = DateTime.Today,
            ExecutionDuration = 42,
            ClientIpAddress = ipAddress,
            ClientName = "MyDesktop",
            BrowserInfo = "Chrome",
            Comments = new List<string> { firstComment, "Second Comment" },
            HttpStatusCode = (int?)HttpStatusCode.BadGateway,
            EntityChanges = {
                    new EntityChangeInfo
                    {
                        EntityId = Guid.NewGuid().ToString(),
                        EntityTypeFullName = "Volo.Abp.AuditLogging.TestEntity_Updated",
                        ChangeType = EntityChangeType.Updated,
                        ChangeTime = updatedEntityChangeTime,
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

        await AuditLogRepository.InsertAsync(await AuditLogInfoToAuditLogConverter.ConvertAsync(log1));
        await AuditLogRepository.InsertAsync(await AuditLogInfoToAuditLogConverter.ConvertAsync(log2));

        //Assert
        var entityChangesDesc = await AuditLogRepository.GetEntityChangeListAsync();
        entityChangesDesc.ShouldNotBeNull();
        entityChangesDesc.Count.ShouldBe(3);

        entityChangesDesc.First().EntityTypeFullName.ShouldBe("Volo.Abp.AuditLogging.TestEntity_Updated");
        entityChangesDesc.Last().EntityTypeFullName.ShouldBe("Volo.Abp.AuditLogging.TestEntity_Deleted");

        var entityChangesAsc = await AuditLogRepository.GetEntityChangeListAsync("changeTime asc");

        entityChangesAsc.First().EntityTypeFullName.ShouldBe("Volo.Abp.AuditLogging.TestEntity_Deleted");
        entityChangesAsc.Last().EntityTypeFullName.ShouldBe("Volo.Abp.AuditLogging.TestEntity_Updated");
    }

    [Fact]
    public async Task GetSpecifiedEntityChangeListAsync()
    {
        // Arrange
        var userId = new Guid("4456fb0d-74cc-4807-9eee-23e551e6cb06");
        var userId2 = new Guid("4456fb0d-74cc-4807-9eee-23e551e6cb06");
        var ipAddress = "153.1.7.61";
        var firstComment = "first Comment";

        var deletedEntityChangeTime = new DateTime(2000, 05, 05, 05, 05, 05);
        var createdEntityChangeTime = new DateTime(2005, 05, 05, 05, 05, 05);
        var updatedEntityChangeTime = new DateTime(2010, 05, 05, 05, 05, 05);

        var log1 = new AuditLogInfo
        {
            UserId = userId,
            ImpersonatorUserId = Guid.NewGuid(),
            ImpersonatorTenantId = Guid.NewGuid(),
            ExecutionTime = DateTime.Today,
            ExecutionDuration = 42,
            ClientIpAddress = ipAddress,
            ClientName = "MyDesktop",
            BrowserInfo = "Chrome",
            Comments = new List<string> { firstComment, "Second Comment" },
            UserName = "Douglas",
            EntityChanges = {
                    new EntityChangeInfo
                {
                    EntityId = Guid.NewGuid().ToString(),
                    EntityTypeFullName = "Volo.Abp.AuditLogging.TestEntity_Deleted",
                    ChangeType = EntityChangeType.Deleted,
                    ChangeTime = deletedEntityChangeTime,
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
                },
                    new EntityChangeInfo
                {
                    EntityId = Guid.NewGuid().ToString(),
                    EntityTypeFullName = "Volo.Abp.AuditLogging.TestEntity_Created",
                    ChangeType = EntityChangeType.Created,
                    ChangeTime = createdEntityChangeTime,
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

        var log2 = new AuditLogInfo
        {
            UserId = userId2,
            ImpersonatorUserId = Guid.NewGuid(),
            ImpersonatorTenantId = Guid.NewGuid(),
            ExecutionTime = DateTime.Today,
            ExecutionDuration = 42,
            ClientIpAddress = ipAddress,
            ClientName = "MyDesktop",
            BrowserInfo = "Chrome",
            Comments = new List<string> { firstComment, "Second Comment" },
            HttpStatusCode = (int?)HttpStatusCode.BadGateway,
            EntityChanges = {
                    new EntityChangeInfo
                    {
                        EntityId = Guid.NewGuid().ToString(),
                        EntityTypeFullName = "Volo.Abp.AuditLogging.TestEntity_Updated",
                        ChangeType = EntityChangeType.Updated,
                        ChangeTime = updatedEntityChangeTime,
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

        await AuditLogRepository.InsertAsync(await AuditLogInfoToAuditLogConverter.ConvertAsync(log1));
        await AuditLogRepository.InsertAsync(await AuditLogInfoToAuditLogConverter.ConvertAsync(log2));

        //Assert
        var entityChanges = await AuditLogRepository.GetEntityChangeListAsync(changeType: EntityChangeType.Created);
        entityChanges.ShouldNotBeNull();
        entityChanges.Count.ShouldBe(1);
    }

    [Fact]
    public async Task GetEntityChangesWithUsernameAsync()
    {
        // Arrange
        var userId = new Guid("4456fb0d-74cc-4807-9eee-23e551e6cb06");
        var userId2 = new Guid("4456fb0d-74cc-4807-9eee-23e551e6cb06");
        var ipAddress = "153.1.7.61";
        var firstComment = "first Comment";

        var firstUser = "Douglas";
        var secondUser = "John Doe";

        var entityId = Guid.NewGuid().ToString();
        var entityType = "Volo.Abp.AuditLogging.TestEntity";

        var log1 = new AuditLogInfo
        {
            UserId = userId,
            ImpersonatorUserId = Guid.NewGuid(),
            ImpersonatorTenantId = Guid.NewGuid(),
            ExecutionTime = DateTime.Today,
            ExecutionDuration = 42,
            ClientIpAddress = ipAddress,
            ClientName = "MyDesktop",
            BrowserInfo = "Chrome",
            Comments = new List<string> { firstComment, "Second Comment" },
            UserName = firstUser,
            EntityChanges = {
                    new EntityChangeInfo
                {
                    EntityId = entityId,
                    EntityTypeFullName = entityType,
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
                },
                    new EntityChangeInfo
                {
                    EntityId = Guid.NewGuid().ToString(),
                    EntityTypeFullName = "Volo.Abp.AuditLogging.TestEntity",
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

        var log2 = new AuditLogInfo
        {
            UserId = userId2,
            ImpersonatorUserId = Guid.NewGuid(),
            ImpersonatorTenantId = Guid.NewGuid(),
            ExecutionTime = DateTime.Today,
            ExecutionDuration = 42,
            ClientIpAddress = ipAddress,
            ClientName = "MyDesktop",
            BrowserInfo = "Chrome",
            Comments = new List<string> { firstComment, "Second Comment" },
            HttpStatusCode = (int?)HttpStatusCode.Accepted,
            UserName = secondUser,
            EntityChanges = {
                    new EntityChangeInfo
                    {
                        EntityId = entityId,
                        EntityTypeFullName = entityType,
                        ChangeType = EntityChangeType.Updated,
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

        await AuditLogRepository.InsertAsync(await AuditLogInfoToAuditLogConverter.ConvertAsync(log1));
        await AuditLogRepository.InsertAsync(await AuditLogInfoToAuditLogConverter.ConvertAsync(log2));

        //Assert
        var entityHistory = await AuditLogRepository.GetEntityChangesWithUsernameAsync(entityId, entityType);

        entityHistory.Count.ShouldBe(2);
        var firstUserChange = entityHistory.First(x => x.UserName == firstUser);
        firstUserChange.ShouldNotBeNull();
        firstUserChange.EntityChange.ShouldNotBeNull();
        firstUserChange.EntityChange.ChangeType.ShouldBe(EntityChangeType.Created);

        var secondUserChange = entityHistory.First(x => x.UserName == secondUser);
        secondUserChange.ShouldNotBeNull();
        secondUserChange.EntityChange.ShouldNotBeNull();
        secondUserChange.EntityChange.ChangeType.ShouldBe(EntityChangeType.Updated);
    }

    [Fact]
    public async Task GetEntityChangeWithUsernameAsync()
    {
        // Arrange
        var userId = new Guid("4456fb0d-74cc-4807-9eee-23e551e6cb06");
        var userId2 = new Guid("4456fb0d-74cc-4807-9eee-23e551e6cb06");
        var ipAddress = "153.1.7.61";
        var firstComment = "first Comment";

        var log1 = new AuditLogInfo
        {
            UserId = userId,
            ImpersonatorUserId = Guid.NewGuid(),
            ImpersonatorTenantId = Guid.NewGuid(),
            ExecutionTime = DateTime.Today,
            ExecutionDuration = 42,
            ClientIpAddress = ipAddress,
            ClientName = "MyDesktop",
            BrowserInfo = "Chrome",
            Comments = new List<string> { firstComment, "Second Comment" },
            UserName = "Douglas",
            EntityChanges = {
                    new EntityChangeInfo
                {
                    EntityId = Guid.NewGuid().ToString(),
                    EntityTypeFullName = "Volo.Abp.AuditLogging.TestEntity_Deleted",
                    ChangeType = EntityChangeType.Deleted,
                    ChangeTime = new DateTime(1995, 3, 27),
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
                },
                    new EntityChangeInfo
                {
                    EntityId = Guid.NewGuid().ToString(),
                    EntityTypeFullName = "Volo.Abp.AuditLogging.TestEntity_Created",
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

        var log2 = new AuditLogInfo
        {
            UserId = userId2,
            ImpersonatorUserId = Guid.NewGuid(),
            ImpersonatorTenantId = Guid.NewGuid(),
            ExecutionTime = DateTime.Today,
            ExecutionDuration = 42,
            ClientIpAddress = ipAddress,
            ClientName = "MyDesktop",
            BrowserInfo = "Chrome",
            Comments = new List<string> { firstComment, "Second Comment" },
            HttpStatusCode = (int?)HttpStatusCode.BadGateway,
            UserName = "John Doe",
            EntityChanges = {
                    new EntityChangeInfo
                    {
                        EntityId = Guid.NewGuid().ToString(),
                        EntityTypeFullName = "Volo.Abp.AuditLogging.TestEntity_Updated",
                        ChangeType = EntityChangeType.Updated,
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

        await AuditLogRepository.InsertAsync(await AuditLogInfoToAuditLogConverter.ConvertAsync(log1));
        await AuditLogRepository.InsertAsync(await AuditLogInfoToAuditLogConverter.ConvertAsync(log2));

        var entityChanges = await AuditLogRepository.GetEntityChangeListAsync();
        var entityHistory =
            await AuditLogRepository.GetEntityChangeWithUsernameAsync(entityChanges.First().Id);

        entityHistory.EntityChange.ChangeTime.ShouldBe(entityChanges.First().ChangeTime);
        entityHistory.UserName.ShouldNotBeNull();
    }
}
