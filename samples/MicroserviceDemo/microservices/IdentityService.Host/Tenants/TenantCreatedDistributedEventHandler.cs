using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Entities.Events.Distributed;
using Volo.Abp.EventBus.Distributed;
using Volo.Abp.MultiTenancy;
using Volo.Abp.TenantManagement;

namespace IdentityService.Host.Tenants
{
    public class TenantCreatedDistributedEventHandler : IDistributedEventHandler<EntityCreatedEto<TenantEto>>, ITransientDependency
    {
        public ILogger<TenantCreatedDistributedEventHandler> Logger { get; set; }
        private readonly IDataSeeder _dataSeeder;
        private readonly ICurrentTenant _currentTenant;

        public TenantCreatedDistributedEventHandler(
            IDataSeeder dataSeeder,
            ICurrentTenant currentTenant)
        {
            _dataSeeder = dataSeeder;
            _currentTenant = currentTenant;
            Logger = NullLogger<TenantCreatedDistributedEventHandler>.Instance;
        }

        public async Task HandleEventAsync(EntityCreatedEto<TenantEto> eventData)
        {
            Logger.LogInformation($"Handled distributed event for a new tenant creation. TenantId: {eventData.Entity.Id}");

            using (_currentTenant.Change(eventData.Entity.Id, eventData.Entity.Name))
            {
                await _dataSeeder.SeedAsync(tenantId: eventData.Entity.Id);
            }
        }
    }
}
