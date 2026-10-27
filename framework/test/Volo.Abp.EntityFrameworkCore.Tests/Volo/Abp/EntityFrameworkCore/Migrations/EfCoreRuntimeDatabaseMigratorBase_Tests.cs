using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Shouldly;
using Volo.Abp.Modularity;
using Volo.Abp.TestApp.Testing;
using Volo.Abp.Uow;
using Xunit;

namespace Volo.Abp.EntityFrameworkCore.Migrations;

// The migration lock (LocalAbpDistributedLock) is process wide, so the migrator tests can not run in parallel.
[Collection(nameof(EfCoreRuntimeDatabaseMigratorBase_Tests))]
public class EfCoreRuntimeDatabaseMigratorBase_Tests : EfCoreRuntimeDatabaseMigratorBase_Tests<RuntimeDatabaseMigratorTestModule>
{
    [Fact]
    public async Task Should_Save_The_Event_To_The_Outbox()
    {
        await CheckAndApplyDatabaseMigrationsAsync();

        GetDirectlyPublishedEventCount().ShouldBe(0);
        (await GetOutboxEventCountAsync()).ShouldBe(1);
    }

    [Fact]
    public async Task Should_Save_The_Event_To_The_Outbox_On_Retry_If_Saving_Fails()
    {
        GetRequiredService<RuntimeDatabaseMigratorTestOutboxSaveFailure>().RemainingFailures = 1;

        await CheckAndApplyDatabaseMigrationsAsync();

        GetDirectlyPublishedEventCount().ShouldBe(0);
        (await GetOutboxEventCountAsync()).ShouldBe(1);
    }

    [Fact]
    public async Task Should_Save_The_Event_To_The_Outbox_On_Retry_If_Another_Instance_Holds_The_Lock()
    {
        GetRequiredService<RuntimeDatabaseMigratorTestOutboxSaveFailure>().RemainingFailures = 1;
        GetRequiredService<RuntimeDatabaseMigratorTestDistributedLock>().ContendedAcquisitionsAfterFirst = 1;

        await CheckAndApplyDatabaseMigrationsAsync();

        GetDirectlyPublishedEventCount().ShouldBe(0);
        (await GetOutboxEventCountAsync()).ShouldBe(1);
    }

    [Fact]
    public async Task Should_Throw_If_The_Event_Is_Not_Saved_Because_Another_Instance_Holds_The_Lock()
    {
        GetRequiredService<RuntimeDatabaseMigratorTestOutboxSaveFailure>().RemainingFailures = 1;
        GetRequiredService<RuntimeDatabaseMigratorTestDistributedLock>().ContendedAcquisitionsAfterFirst = 2;

        await Should.ThrowAsync<AbpException>(CheckAndApplyDatabaseMigrationsAsync());

        GetDirectlyPublishedEventCount().ShouldBe(0);
        (await GetOutboxEventCountAsync()).ShouldBe(0);
    }

    [Fact]
    public async Task Should_Not_Keep_The_Pending_Event_For_The_Next_Call()
    {
        var migrator = GetRequiredService<TestRuntimeDatabaseMigrator>();
        var outboxSaveFailure = GetRequiredService<RuntimeDatabaseMigratorTestOutboxSaveFailure>();

        outboxSaveFailure.RemainingFailures = 3;
        await Should.ThrowAsync<DbUpdateException>(migrator.CheckAndApplyDatabaseMigrationsAsync());

        await migrator.CheckAndApplyDatabaseMigrationsAsync();

        GetDirectlyPublishedEventCount().ShouldBe(0);
        (await GetOutboxEventCountAsync()).ShouldBe(0);
    }

    [Fact]
    public async Task Should_Use_The_Current_Unit_Of_Work()
    {
        using (GetRequiredService<IUnitOfWorkManager>().Begin(requiresNew: true))
        {
            await CheckAndApplyDatabaseMigrationsAsync();

            // Not completed, so the event is discarded with the unit of work.
        }

        GetDirectlyPublishedEventCount().ShouldBe(0);
        (await GetOutboxEventCountAsync()).ShouldBe(0);
    }

    [Fact]
    public async Task Should_Not_Publish_The_Event_Directly_If_It_Can_Not_Be_Saved_To_The_Outbox()
    {
        await WithUnitOfWorkAsync(async () =>
        {
            var dbContext = await GetRequiredService<IDbContextProvider<RuntimeDatabaseMigratorTestDbContext>>().GetDbContextAsync();
            await dbContext.Database.ExecuteSqlRawAsync("DROP TABLE AbpEventOutbox");
        });

        await Should.ThrowAsync<DbUpdateException>(CheckAndApplyDatabaseMigrationsAsync());

        GetDirectlyPublishedEventCount().ShouldBe(0);
    }
}

[Collection(nameof(EfCoreRuntimeDatabaseMigratorBase_Tests))]
public class EfCoreRuntimeDatabaseMigratorBase_WithoutOutbox_Tests : EfCoreRuntimeDatabaseMigratorBase_Tests<RuntimeDatabaseMigratorWithoutOutboxTestModule>
{
    [Fact]
    public async Task Should_Publish_The_Event_Directly()
    {
        await CheckAndApplyDatabaseMigrationsAsync();

        GetDirectlyPublishedEventCount().ShouldBe(1);
        (await GetOutboxEventCountAsync()).ShouldBe(0);
    }

    [Fact]
    public async Task Should_Not_Change_The_Unit_Of_Work_Of_The_Local_Handlers()
    {
        await CheckAndApplyDatabaseMigrationsAsync();

        GetRequiredService<AppliedDatabaseMigrationsRecorder>().IsHandledInTransactionalUnitOfWork.ShouldBe(true);
    }
}

[Collection(nameof(EfCoreRuntimeDatabaseMigratorBase_Tests))]
public class EfCoreRuntimeDatabaseMigratorBase_ExcludedEvent_Tests : EfCoreRuntimeDatabaseMigratorBase_Tests<RuntimeDatabaseMigratorWithExcludedEventTestModule>
{
    [Fact]
    public async Task Should_Publish_The_Event_Directly_If_The_Outbox_Does_Not_Accept_It()
    {
        await CheckAndApplyDatabaseMigrationsAsync();

        GetDirectlyPublishedEventCount().ShouldBe(1);
        (await GetOutboxEventCountAsync()).ShouldBe(0);
        GetRequiredService<AppliedDatabaseMigrationsRecorder>().IsHandledInTransactionalUnitOfWork.ShouldBe(true);
    }
}

public abstract class EfCoreRuntimeDatabaseMigratorBase_Tests<TStartupModule> : TestAppTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{
    protected Task CheckAndApplyDatabaseMigrationsAsync()
    {
        return GetRequiredService<TestRuntimeDatabaseMigrator>().CheckAndApplyDatabaseMigrationsAsync();
    }

    protected int GetDirectlyPublishedEventCount()
    {
        return GetRequiredService<AppliedDatabaseMigrationsRecorder>().Count;
    }

    protected Task<int> GetOutboxEventCountAsync()
    {
        return WithUnitOfWorkAsync(async () =>
        {
            var dbContext = await GetRequiredService<IDbContextProvider<RuntimeDatabaseMigratorTestDbContext>>().GetDbContextAsync();
            return await dbContext.OutgoingEvents.CountAsync();
        });
    }
}
