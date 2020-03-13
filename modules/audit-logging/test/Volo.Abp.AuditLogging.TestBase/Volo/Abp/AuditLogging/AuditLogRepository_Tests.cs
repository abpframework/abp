using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Shouldly;
using Volo.Abp.Auditing;
using Volo.Abp.Guids;
using Volo.Abp.Modularity;
using Xunit;

namespace Volo.Abp.AuditLogging
{
    public abstract class AuditLogRepository_Tests<TStartupModule> : AuditLoggingTestBase<TStartupModule>
        where TStartupModule : IAbpModule
    {
        protected IAuditLogRepository AuditLogRepository { get; }
        protected IGuidGenerator GuidGenerator { get; }

        protected AuditLogRepository_Tests()
        {
            AuditLogRepository = GetRequiredService<IAuditLogRepository>();
            GuidGenerator = GetRequiredService<IGuidGenerator>();
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

            await AuditLogRepository.InsertAsync(new AuditLog(GuidGenerator, log1));
            await AuditLogRepository.InsertAsync(new AuditLog(GuidGenerator, log2));

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

            await AuditLogRepository.InsertAsync(new AuditLog(GuidGenerator, log1));
            await AuditLogRepository.InsertAsync(new AuditLog(GuidGenerator, log2));

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

            await AuditLogRepository.InsertAsync(new AuditLog(GuidGenerator, log1));
            await AuditLogRepository.InsertAsync(new AuditLog(GuidGenerator, log2));

            //Assert
            var date = DateTime.Parse("2020-01-01");
            var results = await AuditLogRepository.GetAverageExecutionDurationPerDayAsync(date, date);
            results.Count.ShouldBe(1);
            results.Values.First().ShouldBe(50); // (45 + 55) / 2
        }
    }
}
