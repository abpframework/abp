using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.DistributedLocking;
using Volo.Abp.Domain.Entities.Events.Distributed;
using Volo.Abp.EventBus.Distributed;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Uow;

namespace Volo.Abp.EntityFrameworkCore.Migrations;

public abstract class EfCoreDatabaseMigrationEventHandlerBase<TDbContext> :
    IDistributedEventHandler<TenantCreatedEto>,
    IDistributedEventHandler<TenantConnectionStringUpdatedEto>,
    IDistributedEventHandler<ApplyDatabaseMigrationsEto>,
    ITransientDependency
    where TDbContext : DbContext, IEfCoreDbContext
{
    protected string DatabaseName { get; }

    protected const string TryCountPropertyName = "__TryCount";

    protected int MaxEventTryCount { get; set; } = 3;

    /// <summary>
    /// As milliseconds.
    /// </summary>
    protected int MinValueToWaitOnFailure { get; set; } = 5000;

    /// <summary>
    /// As milliseconds.
    /// </summary>
    protected int MaxValueToWaitOnFailure { get; set; } = 15000;

    /// <summary>
    /// As minutes.
    /// </summary>
    protected TimeSpan DistributedLockAcquireTimeout { get; set; } = TimeSpan.FromMinutes(15);

    protected ICurrentTenant CurrentTenant { get; }
    protected IUnitOfWorkManager UnitOfWorkManager { get; }
    protected ITenantStore TenantStore { get; }
    protected IDistributedEventBus DistributedEventBus { get; }
    protected IAbpDistributedLock DistributedLock { get; }
    protected ILogger<EfCoreDatabaseMigrationEventHandlerBase<TDbContext>> Logger { get; }

    protected EfCoreDatabaseMigrationEventHandlerBase(
        string databaseName,
        ICurrentTenant currentTenant,
        IUnitOfWorkManager unitOfWorkManager,
        ITenantStore tenantStore,
        IAbpDistributedLock abpDistributedLock,
        IDistributedEventBus distributedEventBus,
        ILoggerFactory loggerFactory)
    {
        CurrentTenant = currentTenant;
        UnitOfWorkManager = unitOfWorkManager;
        TenantStore = tenantStore;
        DatabaseName = databaseName;
        DistributedLock = abpDistributedLock;
        DistributedEventBus = distributedEventBus;

        Logger = loggerFactory.CreateLogger<EfCoreDatabaseMigrationEventHandlerBase<TDbContext>>();
    }

    public virtual async Task HandleEventAsync(ApplyDatabaseMigrationsEto eventData)
    {
        if (eventData.DatabaseName != DatabaseName)
        {
            return;
        }

        var schemaMigrated = false;
        try
        {
            schemaMigrated = await MigrateDatabaseSchemaAsync(eventData.TenantId);
            await SeedAsync(eventData.TenantId);

            if (schemaMigrated)
            {
                await DistributedEventBus.PublishAsync(
                    new AppliedDatabaseMigrationsEto
                    {
                        DatabaseName = DatabaseName,
                        TenantId = eventData.TenantId
                    }
                );
            }
        }
        catch (Exception ex)
        {
            await HandleErrorOnApplyDatabaseMigrationAsync(eventData, ex);
        }

        await AfterApplyDatabaseMigrations(eventData, schemaMigrated);
    }

    protected virtual Task AfterApplyDatabaseMigrations(ApplyDatabaseMigrationsEto eventData, bool schemaMigrated)
    {
        return Task.CompletedTask;
    }

    public virtual async Task HandleEventAsync(TenantCreatedEto eventData)
    {
        var schemaMigrated = false;
        try
        {
            schemaMigrated = await MigrateDatabaseSchemaAsync(eventData.Id);
            await SeedAsync(eventData.Id);

            if (schemaMigrated)
            {
                await DistributedEventBus.PublishAsync(
                    new AppliedDatabaseMigrationsEto
                    {
                        DatabaseName = DatabaseName,
                        TenantId = eventData.Id
                    }
                );
            }
        }
        catch (Exception ex)
        {
            await HandleErrorTenantCreatedAsync(eventData, ex);
        }

        await AfterTenantCreated(eventData, schemaMigrated);
    }

    protected virtual Task AfterTenantCreated(TenantCreatedEto eventData, bool schemaMigrated)
    {
        return Task.CompletedTask;
    }

    public virtual async Task HandleEventAsync(TenantConnectionStringUpdatedEto eventData)
    {
        if (eventData.ConnectionStringName != DatabaseName &&
            eventData.ConnectionStringName != Volo.Abp.Data.ConnectionStrings.DefaultConnectionStringName ||
            eventData.NewValue.IsNullOrWhiteSpace())
        {
            return;
        }

        var schemaMigrated = false;
        try
        {
            schemaMigrated = await MigrateDatabaseSchemaAsync(eventData.Id);
            await SeedAsync(eventData.Id);

            if (schemaMigrated)
            {
                await DistributedEventBus.PublishAsync(
                    new AppliedDatabaseMigrationsEto
                    {
                        DatabaseName = DatabaseName,
                        TenantId = eventData.Id
                    }
                );
            }
        }
        catch (Exception ex)
        {
            await HandleErrorTenantConnectionStringUpdatedAsync(eventData, ex);
        }

        await AfterTenantConnectionStringUpdated(eventData, schemaMigrated);
    }

    protected virtual Task AfterTenantConnectionStringUpdated(TenantConnectionStringUpdatedEto eventData,
        bool schemaMigrated)
    {
        return Task.CompletedTask;
    }

    protected virtual Task SeedAsync(Guid? tenantId)
    {
        return Task.CompletedTask;
    }

    /// <summary>
    /// Apply pending EF Core schema migrations to the database.
    /// Returns true if any migration has applied.
    /// </summary>
    protected virtual async Task<bool> MigrateDatabaseSchemaAsync(Guid? tenantId)
    {
        var result = false;

        using (CurrentTenant.Change(tenantId))
        {
            using (var uow = UnitOfWorkManager.Begin(requiresNew: true, isTransactional: false))
            {
                async Task<IAsyncDisposable> WaitForDistributedLockAsync(TDbContext dbContext)
                {
                    var distributedLockHandle = await DistributedLock.TryAcquireAsync(
                        "DatabaseMigrationEventHandler_" +
                        dbContext.Database.GetConnectionString()!.ToUpperInvariant().ToMd5(),
                        DistributedLockAcquireTimeout
                    );

                    if(distributedLockHandle == null)
                    {
                        throw new AbpException($"Distributed lock could not be acquired for database migration event handler: {DatabaseName}.");
                    }

                    return distributedLockHandle;
                }

                async Task<bool> MigrateDatabaseSchemaWithDbContextAsync()
                {
                    var dbContext = await uow.ServiceProvider
                        .GetRequiredService<IDbContextProvider<TDbContext>>()
                        .GetDbContextAsync();

                    await using (await WaitForDistributedLockAsync(dbContext))
                    {
                        if ((await dbContext.Database.GetPendingMigrationsAsync()).Any())
                        {
                            await dbContext.Database.MigrateAsync();
                            return true;
                        }
                    }

                    return false;
                }

                if (tenantId == null)
                {
                    //Migrating the host database
                    Logger.LogInformation($"Migrating database of host. Database Name = {DatabaseName}");
                    result = await MigrateDatabaseSchemaWithDbContextAsync();
                }
                else
                {
                    var tenantConfiguration = await TenantStore.FindAsync(tenantId.Value);
                    if (!tenantConfiguration!.ConnectionStrings!.Default.IsNullOrWhiteSpace() ||
                        !tenantConfiguration.ConnectionStrings.GetOrDefault(DatabaseName).IsNullOrWhiteSpace())
                    {
                        //Migrating the tenant database (only if tenant has a separate database)
                        Logger.LogInformation(
                            $"Migrating separate database of tenant. Database Name = {DatabaseName}, TenantId = {tenantId}");
                        result = await MigrateDatabaseSchemaWithDbContextAsync();
                    }
                }

                await uow.CompleteAsync();
            }
        }

        return result;
    }

    protected virtual async Task HandleErrorOnApplyDatabaseMigrationAsync(
        ApplyDatabaseMigrationsEto eventData,
        Exception exception)
    {
        var tryCount = IncrementEventTryCount(eventData);
        if (tryCount <= MaxEventTryCount)
        {
            Logger.LogWarning(
                $"Could not apply database migrations. Re-queueing the operation. TenantId = {eventData.TenantId}, Database Name = {eventData.DatabaseName}.");
            Logger.LogException(exception, LogLevel.Warning);

            await Task.Delay(RandomHelper.GetRandom(MinValueToWaitOnFailure, MaxValueToWaitOnFailure));
            await DistributedEventBus.PublishAsync(eventData);
        }
        else
        {
            Logger.LogError(
                $"Could not apply database migrations. Canceling the operation. TenantId = {eventData.TenantId}, DatabaseName = {eventData.DatabaseName}.");
            Logger.LogException(exception);
        }
    }

    protected virtual async Task HandleErrorTenantCreatedAsync(
        TenantCreatedEto eventData,
        Exception exception)
    {
        var tryCount = IncrementEventTryCount(eventData);
        if (tryCount <= MaxEventTryCount)
        {
            Logger.LogWarning(
                $"Could not perform tenant created event. Re-queueing the operation. TenantId = {eventData.Id}, TenantName = {eventData.Name}.");
            Logger.LogException(exception, LogLevel.Warning);

            await Task.Delay(RandomHelper.GetRandom(5000, 15000));
            await DistributedEventBus.PublishAsync(eventData);
        }
        else
        {
            Logger.LogError(
                $"Could not perform tenant created event. Canceling the operation. TenantId = {eventData.Id}, TenantName = {eventData.Name}.");
            Logger.LogException(exception);
        }
    }

    protected virtual async Task HandleErrorTenantConnectionStringUpdatedAsync(
        TenantConnectionStringUpdatedEto eventData,
        Exception exception)
    {
        var tryCount = IncrementEventTryCount(eventData);
        if (tryCount <= MaxEventTryCount)
        {
            Logger.LogWarning(
                $"Could not perform tenant connection string updated event. Re-queueing the operation. TenantId = {eventData.Id}, TenantName = {eventData.Name}.");
            Logger.LogException(exception, LogLevel.Warning);

            await Task.Delay(RandomHelper.GetRandom(5000, 15000));
            await DistributedEventBus.PublishAsync(eventData);
        }
        else
        {
            Logger.LogError(
                $"Could not perform tenant connection string updated event. Canceling the operation. TenantId = {eventData.Id}, TenantName = {eventData.Name}.");
            Logger.LogException(exception);
        }
    }

    private static int GetEventTryCount(EtoBase eventData)
    {
        var tryCountAsString = eventData.Properties.GetOrDefault(TryCountPropertyName);
        if (tryCountAsString.IsNullOrEmpty())
        {
            return 0;
        }

        return int.Parse(tryCountAsString!);
    }

    private static void SetEventTryCount(EtoBase eventData, int count)
    {
        eventData.Properties[TryCountPropertyName] = count.ToString();
    }

    private static int IncrementEventTryCount(EtoBase eventData)
    {
        var count = GetEventTryCount(eventData) + 1;
        SetEventTryCount(eventData, count);
        return count;
    }
}