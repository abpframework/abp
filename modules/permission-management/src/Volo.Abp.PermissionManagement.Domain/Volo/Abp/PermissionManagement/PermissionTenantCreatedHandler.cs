using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EventBus.Distributed;
using Volo.Abp.MultiTenancy;

namespace Volo.Abp.PermissionManagement
{
    public class PermissionTenantCreatedHandler : IDistributedEventHandler<TenantCreatedEto>, ITransientDependency
    {
        protected IPermissionDefinitionManager PermissionDefinitionManager { get; }
        protected IPermissionDataSeeder PermissionDataSeeder { get; }

        public PermissionTenantCreatedHandler(IPermissionDefinitionManager permissionDefinitionManager, IPermissionDataSeeder permissionDataSeeder)
        {
            PermissionDefinitionManager = permissionDefinitionManager;
            PermissionDataSeeder = permissionDataSeeder;
        }

        public async Task HandleEventAsync(TenantCreatedEto eventData)
        {
            var permissionNames = PermissionDefinitionManager
                .GetPermissions()
                .Where(p => p.MultiTenancySide.HasFlag(MultiTenancySides.Tenant))
                .Where(p => !p.Providers.Any() || p.Providers.Contains(RolePermissionValueProvider.ProviderName))
                .Select(p => p.Name)
                .ToArray();

            await PermissionDataSeeder.SeedAsync(
                RolePermissionValueProvider.ProviderName,
                "admin",
                permissionNames,
                eventData.Id
            );
        }
    }
}
