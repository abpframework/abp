using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.PermissionManagement
{
    public class PermissionDataSeedContributor : IDataSeedContributor, ITransientDependency
    {
        protected IPermissionDefinitionManager PermissionDefinitionManager { get; }
        protected IPermissionDataSeeder PermissionDataSeeder { get; }

        public PermissionDataSeedContributor(
            IPermissionDefinitionManager permissionDefinitionManager,
            IPermissionDataSeeder permissionDataSeeder)
        {
            PermissionDefinitionManager = permissionDefinitionManager;
            PermissionDataSeeder = permissionDataSeeder;
        }

        public Task SeedAsync(DataSeedContext context)
        {
            var permissionNames = PermissionDefinitionManager
                .GetPermissions()
                .Select(p => p.Name)
                //TODO: Filter host/tenant permissions!
                .ToArray();

            return PermissionDataSeeder.SeedAsync(
                RolePermissionValueProvider.ProviderName,
                "admin",
                permissionNames,
                context.TenantId
            );
        }
    }
}
