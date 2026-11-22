using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Shouldly;
using Volo.Abp.Data;
using Volo.Abp.DistributedLocking;
using Volo.Abp.EventBus.Distributed;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Uow;
using Xunit;

namespace Volo.Abp.EntityFrameworkCore.Migrations;

public class EfCoreDatabaseMigrationEventHandlerBase_Tests : EntityFrameworkCoreTestBase
{
    // A tenant can be deleted before the services handle the events about it: the tenant store then returns null.

    [Fact]
    public async Task Should_Skip_The_Tenant_Created_Event_Of_A_Deleted_Tenant()
    {
        var handler = CreateHandler();

        await handler.HandleEventAsync(new TenantCreatedEto { Id = Guid.NewGuid(), Name = "deleted-tenant" });

        handler.SeededTenantIds.ShouldBeEmpty();
        handler.Errors.ShouldBeEmpty();
    }

    [Fact]
    public async Task Should_Skip_The_Connection_String_Updated_Event_Of_A_Deleted_Tenant()
    {
        var handler = CreateHandler();

        await handler.HandleEventAsync(new TenantConnectionStringUpdatedEto
        {
            Id = Guid.NewGuid(),
            Name = "deleted-tenant",
            ConnectionStringName = TestDatabaseMigrationEventHandler.TestDatabaseName,
            NewValue = "Data Source=deleted-tenant.db"
        });

        handler.SeededTenantIds.ShouldBeEmpty();
        handler.Errors.ShouldBeEmpty();
    }

    [Fact]
    public async Task Should_Skip_The_Apply_Database_Migrations_Event_Of_A_Deleted_Tenant()
    {
        var handler = CreateHandler();

        await handler.HandleEventAsync(new ApplyDatabaseMigrationsEto
        {
            TenantId = Guid.NewGuid(),
            DatabaseName = TestDatabaseMigrationEventHandler.TestDatabaseName
        });

        handler.SeededTenantIds.ShouldBeEmpty();
        handler.Errors.ShouldBeEmpty();
    }

    private TestDatabaseMigrationEventHandler CreateHandler()
    {
        return new TestDatabaseMigrationEventHandler(
            GetRequiredService<ICurrentTenant>(),
            GetRequiredService<IUnitOfWorkManager>(),
            GetRequiredService<ITenantStore>(),
            GetRequiredService<IAbpDistributedLock>(),
            GetRequiredService<IDistributedEventBus>(),
            GetRequiredService<ILoggerFactory>()
        );
    }

    private class TestDatabaseMigrationEventHandler : EfCoreDatabaseMigrationEventHandlerBase<TestMigrationsDbContext>
    {
        public const string TestDatabaseName = "TestMigrations";

        public List<Guid?> SeededTenantIds { get; } = new();

        public List<Exception> Errors { get; } = new();

        public TestDatabaseMigrationEventHandler(
            ICurrentTenant currentTenant,
            IUnitOfWorkManager unitOfWorkManager,
            ITenantStore tenantStore,
            IAbpDistributedLock abpDistributedLock,
            IDistributedEventBus distributedEventBus,
            ILoggerFactory loggerFactory)
            : base(TestDatabaseName, currentTenant, unitOfWorkManager, tenantStore, abpDistributedLock, distributedEventBus, loggerFactory)
        {
        }

        protected override Task SeedAsync(Guid? tenantId)
        {
            SeededTenantIds.Add(tenantId);
            return Task.CompletedTask;
        }

        // Recorded instead of re-queued, so that a failure shows up at once.
        protected override Task HandleErrorTenantCreatedAsync(TenantCreatedEto eventData, Exception exception)
        {
            Errors.Add(exception);
            return Task.CompletedTask;
        }

        protected override Task HandleErrorTenantConnectionStringUpdatedAsync(TenantConnectionStringUpdatedEto eventData, Exception exception)
        {
            Errors.Add(exception);
            return Task.CompletedTask;
        }

        protected override Task HandleErrorOnApplyDatabaseMigrationAsync(ApplyDatabaseMigrationsEto eventData, Exception exception)
        {
            Errors.Add(exception);
            return Task.CompletedTask;
        }
    }
}
