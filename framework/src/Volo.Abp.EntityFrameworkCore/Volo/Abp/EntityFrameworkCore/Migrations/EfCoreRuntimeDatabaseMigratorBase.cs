using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.DistributedLocking;
using Volo.Abp.EventBus.Distributed;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Uow;

namespace Volo.Abp.EntityFrameworkCore.Migrations;

public abstract class EfCoreRuntimeDatabaseMigratorBase<TDbContext> : ITransientDependency 
    where TDbContext : DbContext, IEfCoreDbContext
{
    protected int MinValueToWaitOnFailure { get; set; } = 5000;
    protected int MaxValueToWaitOnFailure { get; set; } = 15000;

    protected string DatabaseName { get; }
    
    /// <summary>
    /// Enabling this might be inefficient if you have many tenants!
    /// If disabled (default), tenant databases will be seeded only
    /// if there is a schema migration applied to the host database.
    /// If enabled, tenant databases will be seeded always on every service startup.
    /// </summary>
    protected bool AlwaysSeedTenantDatabases { get; set; } = false;

    protected IUnitOfWorkManager UnitOfWorkManager { get; }
    protected IServiceProvider ServiceProvider { get; }
    protected ICurrentTenant CurrentTenant { get; }
    protected IAbpDistributedLock DistributedLock { get; }
    protected IDistributedEventBus DistributedEventBus { get; }
    protected ILogger<EfCoreRuntimeDatabaseMigratorBase<TDbContext>> Logger { get; }
    
    protected EfCoreRuntimeDatabaseMigratorBase(
        string databaseName,
        IUnitOfWorkManager unitOfWorkManager,
        IServiceProvider serviceProvider,
        ICurrentTenant currentTenant,
        IAbpDistributedLock abpDistributedLock,
        IDistributedEventBus distributedEventBus,
        ILoggerFactory loggerFactory)
    {
        DatabaseName = databaseName;
        UnitOfWorkManager = unitOfWorkManager;
        ServiceProvider = serviceProvider;
        CurrentTenant = currentTenant;
        DistributedLock = abpDistributedLock;
        DistributedEventBus = distributedEventBus;
        Logger = loggerFactory.CreateLogger<EfCoreRuntimeDatabaseMigratorBase<TDbContext>>();
    }

    public virtual async Task CheckAndApplyDatabaseMigrationsAsync()
    {
        await TryAsync(LockAndApplyDatabaseMigrationsAsync);
    }

    protected virtual async Task LockAndApplyDatabaseMigrationsAsync()
    {
        Logger.LogInformation($"Trying to acquire the distributed lock for database migration: {DatabaseName}.");

        var schemaMigrated = false;
        
        await using (var handle = await DistributedLock.TryAcquireAsync("DatabaseMigration_" + DatabaseName))
        {
            if (handle is null)
            {
                Logger.LogInformation($"Distributed lock could not be acquired for database migration: {DatabaseName}. Operation cancelled.");
                return;
            }
            
            Logger.LogInformation($"Distributed lock is acquired for database migration: {DatabaseName}...");

            using (CurrentTenant.Change(null))
            {
                // Create database tables if needed
                using (var uow = UnitOfWorkManager.Begin(requiresNew: true, isTransactional: false))
                {
                    var dbContext = await ServiceProvider
                        .GetRequiredService<IDbContextProvider<TDbContext>>()
                        .GetDbContextAsync();

                    var pendingMigrations = await dbContext
                        .Database
                        .GetPendingMigrationsAsync();

                    if (pendingMigrations.Any())
                    {
                        await dbContext.Database.MigrateAsync();
                        schemaMigrated = true;
                    }

                    await uow.CompleteAsync();
                }
            }

            await SeedAsync();
            
            if (schemaMigrated || AlwaysSeedTenantDatabases)
            {
                await DistributedEventBus.PublishAsync(
                    new AppliedDatabaseMigrationsEto
                    {
                        DatabaseName = DatabaseName,
                        TenantId = null
                    }
                );
            }
        }
        
        Logger.LogInformation($"Distributed lock has been released for database migration: {DatabaseName}...");
    }

    protected virtual Task SeedAsync()
    {
        return Task.CompletedTask;
    }

    protected virtual async Task TryAsync(Func<Task> task, int maxTryCount = 3)
    {
        try
        {
            await task();
        }
        catch (Exception ex)
        {
            maxTryCount--;

            if (maxTryCount <= 0)
            {
                throw;
            }

            Logger.LogWarning(ex, $"{ex.GetType().Name} has been thrown. The operation will be tried {maxTryCount} times more.");

            await Task.Delay(RandomHelper.GetRandom(MinValueToWaitOnFailure, MaxValueToWaitOnFailure));

            await TryAsync(task, maxTryCount);
        }
    }
}