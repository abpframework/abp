using System;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EventBus.Distributed;
using Volo.Abp.MultiTenancy;

namespace Volo.Abp.MongoDB.Migrations;

public abstract class MongoDatabaseMigrationEventHandlerBase<TDbContext> :
    IDistributedEventHandler<TenantCreatedEto>,
    IDistributedEventHandler<TenantConnectionStringUpdatedEto>,
    ITransientDependency
    where TDbContext : IAbpMongoDbContext
{
    protected string DatabaseName { get; }


    protected MongoDatabaseMigrationEventHandlerBase(string databaseName)
    {
        DatabaseName = databaseName;
    }

    public virtual async Task HandleEventAsync(TenantCreatedEto eventData)
    {
        await SeedAsync(eventData.Id);

        await AfterTenantCreated(eventData);
    }

    protected virtual Task AfterTenantCreated(TenantCreatedEto eventData)
    {
        return Task.CompletedTask;
    }

    public virtual async Task HandleEventAsync(TenantConnectionStringUpdatedEto eventData)
    {
        if (eventData.ConnectionStringName != DatabaseName &&
            eventData.ConnectionStringName != Data.ConnectionStrings.DefaultConnectionStringName ||
            eventData.NewValue.IsNullOrWhiteSpace())
        {
            return;
        }

        await SeedAsync(eventData.Id);

        await AfterTenantConnectionStringUpdated(eventData);
    }

    protected virtual Task AfterTenantConnectionStringUpdated(TenantConnectionStringUpdatedEto eventData)
    {
        return Task.CompletedTask;
    }

    protected virtual Task SeedAsync(Guid? tenantId)
    {
        return Task.CompletedTask;
    }
}