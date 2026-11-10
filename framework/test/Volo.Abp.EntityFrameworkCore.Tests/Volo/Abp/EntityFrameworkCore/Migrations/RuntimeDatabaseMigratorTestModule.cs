using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Volo.Abp.Autofac;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.DistributedLocking;
using Volo.Abp.EntityFrameworkCore.DistributedEvents;
using Volo.Abp.EntityFrameworkCore.Sqlite;
using Volo.Abp.EventBus.Distributed;
using Volo.Abp.Modularity;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Uow;

namespace Volo.Abp.EntityFrameworkCore.Migrations;

[DependsOn(typeof(AbpEntityFrameworkCoreSqliteModule), typeof(AbpAutofacModule))]
public class RuntimeDatabaseMigratorTestModule : RuntimeDatabaseMigratorTestModuleBase
{
    protected override void ConfigureOutboxes(OutboxConfigDictionary outboxes)
    {
        outboxes.Configure(config =>
        {
            config.UseDbContext<RuntimeDatabaseMigratorTestDbContext>();
            // Keep the events in the outbox, so the tests can check them.
            config.IsSendingEnabled = false;
        });
    }
}

[DependsOn(typeof(AbpEntityFrameworkCoreSqliteModule), typeof(AbpAutofacModule))]
public class RuntimeDatabaseMigratorWithoutOutboxTestModule : RuntimeDatabaseMigratorTestModuleBase
{
}

[DependsOn(typeof(AbpEntityFrameworkCoreSqliteModule), typeof(AbpAutofacModule))]
public class RuntimeDatabaseMigratorWithExcludedEventTestModule : RuntimeDatabaseMigratorTestModuleBase
{
    protected override void ConfigureOutboxes(OutboxConfigDictionary outboxes)
    {
        outboxes.Configure(config =>
        {
            config.UseDbContext<RuntimeDatabaseMigratorTestDbContext>();
            config.IsSendingEnabled = false;
            config.Selector = eventType => eventType != typeof(AppliedDatabaseMigrationsEto);
        });
    }
}

public abstract class RuntimeDatabaseMigratorTestModuleBase : AbpModule
{
    private readonly AbpUnitTestSqliteDatabase _database = new();
    private readonly RuntimeDatabaseMigratorTestOutboxSaveFailure _outboxSaveFailure = new();

    protected virtual void ConfigureOutboxes(OutboxConfigDictionary outboxes)
    {
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddAbpDbContext<RuntimeDatabaseMigratorTestDbContext>();
        context.Services.AddSingleton(_outboxSaveFailure);
        context.Services.Replace(ServiceDescriptor.Singleton<IAbpDistributedLock, RuntimeDatabaseMigratorTestDistributedLock>());
        context.Services.AddSingleton(provider => (RuntimeDatabaseMigratorTestDistributedLock)provider.GetRequiredService<IAbpDistributedLock>());

        _database.CreateTables(new RuntimeDatabaseMigratorTestDbContext(
            new DbContextOptionsBuilder<RuntimeDatabaseMigratorTestDbContext>().UseSqlite(_database.ConnectionString).Options));

        Configure<AbpDbConnectionOptions>(options =>
        {
            options.ConnectionStrings[RuntimeDatabaseMigratorTestDbContext.DatabaseName] = _database.ConnectionString;
        });

        Configure<AbpDbContextOptions>(options =>
        {
            options.Configure<RuntimeDatabaseMigratorTestDbContext>(configuration =>
            {
                configuration.UseSqlite(sqlite =>
                {
                    sqlite.ExecutionStrategy(dependencies => new RuntimeDatabaseMigratorTestRetryingExecutionStrategy(dependencies));
                });
                configuration.DbContextOptions.AddInterceptors(_outboxSaveFailure);
            });
        });

        Configure<AbpDistributedEventBusOptions>(options => ConfigureOutboxes(options.Outboxes));
    }

    public override void OnApplicationShutdown(ApplicationShutdownContext context)
    {
        _database.Dispose();
    }
}

[ConnectionStringName(DatabaseName)]
public class RuntimeDatabaseMigratorTestDbContext : AbpDbContext<RuntimeDatabaseMigratorTestDbContext>, IHasEventOutbox
{
    public const string DatabaseName = "RuntimeDatabaseMigratorTest";

    public DbSet<OutgoingEventRecord> OutgoingEvents { get; set; }

    public RuntimeDatabaseMigratorTestDbContext(DbContextOptions<RuntimeDatabaseMigratorTestDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ConfigureEventOutbox();
    }
}

// A real pending migration, so the migrator publishes the event like after a deployment.
[DbContext(typeof(RuntimeDatabaseMigratorTestDbContext))]
[Migration("20261017000000_RuntimeDatabaseMigratorTest")]
public class RuntimeDatabaseMigratorTestMigration : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.Sql("CREATE TABLE RuntimeDatabaseMigratorTestMarkers (Id INTEGER NOT NULL)");
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.Sql("DROP TABLE RuntimeDatabaseMigratorTestMarkers");
    }
}

public class TestRuntimeDatabaseMigrator : EfCoreRuntimeDatabaseMigratorBase<RuntimeDatabaseMigratorTestDbContext>
{
    public TestRuntimeDatabaseMigrator(
        IUnitOfWorkManager unitOfWorkManager,
        IServiceProvider serviceProvider,
        ICurrentTenant currentTenant,
        IAbpDistributedLock abpDistributedLock,
        IDistributedEventBus distributedEventBus,
        ILoggerFactory loggerFactory)
        : base(
            RuntimeDatabaseMigratorTestDbContext.DatabaseName,
            unitOfWorkManager,
            serviceProvider,
            currentTenant,
            abpDistributedLock,
            distributedEventBus,
            loggerFactory)
    {
        MinValueToWaitOnFailure = 0;
        MaxValueToWaitOnFailure = 1;
    }
}

public class AppliedDatabaseMigrationsRecorder : IDistributedEventHandler<AppliedDatabaseMigrationsEto>, ISingletonDependency
{
    private readonly IUnitOfWorkManager _unitOfWorkManager;
    private int _count;

    public int Count => _count;

    public bool? IsHandledInTransactionalUnitOfWork { get; private set; }

    public AppliedDatabaseMigrationsRecorder(IUnitOfWorkManager unitOfWorkManager)
    {
        _unitOfWorkManager = unitOfWorkManager;
    }

    [UnitOfWork(isTransactional: true)]
    public virtual Task HandleEventAsync(AppliedDatabaseMigrationsEto eventData)
    {
        IsHandledInTransactionalUnitOfWork = _unitOfWorkManager.Current?.Options.IsTransactional;
        Interlocked.Increment(ref _count);
        return Task.CompletedTask;
    }
}

public class RuntimeDatabaseMigratorTestOutboxSaveFailure : SaveChangesInterceptor
{
    public int RemainingFailures { get; set; }

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default)
    {
        if (RemainingFailures > 0 &&
            eventData.Context.ChangeTracker.Entries<OutgoingEventRecord>().Any(x => x.State == EntityState.Added))
        {
            RemainingFailures--;
            throw new DbUpdateException("Could not save the outbox event.");
        }

        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }
}

// Simulates another instance that holds the lock after the first acquisition.
public class RuntimeDatabaseMigratorTestDistributedLock : IAbpDistributedLock
{
    private readonly LocalAbpDistributedLock _localDistributedLock;
    private int _acquisitionCount;

    public int ContendedAcquisitionsAfterFirst { get; set; }

    public RuntimeDatabaseMigratorTestDistributedLock(LocalAbpDistributedLock localDistributedLock)
    {
        _localDistributedLock = localDistributedLock;
    }

    public Task<IAbpDistributedLockHandle> TryAcquireAsync(string name, TimeSpan timeout = default, CancellationToken cancellationToken = default)
    {
        if (++_acquisitionCount > 1 && ContendedAcquisitionsAfterFirst > 0)
        {
            ContendedAcquisitionsAfterFirst--;
            return Task.FromResult<IAbpDistributedLockHandle>(null);
        }

        return _localDistributedLock.TryAcquireAsync(name, timeout, cancellationToken);
    }
}

// Rejects user-initiated transactions, like SQL Server's EnableRetryOnFailure.
public class RuntimeDatabaseMigratorTestRetryingExecutionStrategy : ExecutionStrategy
{
    public RuntimeDatabaseMigratorTestRetryingExecutionStrategy(ExecutionStrategyDependencies dependencies)
        : base(dependencies, maxRetryCount: 3, maxRetryDelay: TimeSpan.FromMilliseconds(10))
    {
    }

    protected override bool ShouldRetryOn(Exception exception)
    {
        return false;
    }
}
