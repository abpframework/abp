using System.Threading.Tasks;
using AutoMapper.Internal;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EventBus.Distributed;
using Volo.Abp.MultiTenancy;

namespace Volo.Abp.Identity
{
    public class IdentityTenantCreatedHandler : IDistributedEventHandler<TenantCreatedEto>, ITransientDependency
    {
        public ILogger<IdentityTenantCreatedHandler> Logger { get; set; }

        protected IIdentityDataSeeder IdentityDataSeeder { get; }

        public IdentityTenantCreatedHandler(IIdentityDataSeeder identityDataSeeder)
        {
            IdentityDataSeeder = identityDataSeeder;
            Logger = NullLogger<IdentityTenantCreatedHandler>.Instance;
        }

        public async Task HandleEventAsync(TenantCreatedEto eventData)
        {
            await IdentityDataSeeder.SeedAsync(
                eventData.Properties.GetOrDefault("AdminEmail") as string ?? "admin@abp.io",
                eventData.Properties.GetOrDefault("AdminPassword") as string ?? "1q2w3E*",
                eventData.Id
            );
        }
    }
}
