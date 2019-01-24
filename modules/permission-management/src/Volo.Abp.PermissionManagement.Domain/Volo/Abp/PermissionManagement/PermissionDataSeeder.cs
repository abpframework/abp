using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Guids;

namespace Volo.Abp.PermissionManagement
{
    public class PermissionDataSeeder : IPermissionDataSeeder, ITransientDependency
    {
        protected IPermissionGrantRepository PermissionGrantRepository { get; }
        protected IGuidGenerator GuidGenerator { get; }

        public PermissionDataSeeder(
            IPermissionGrantRepository permissionGrantRepository, 
            IGuidGenerator guidGenerator)
        {
            PermissionGrantRepository = permissionGrantRepository;
            GuidGenerator = guidGenerator;
        }

        public async Task SeedAsync(
            string providerName, 
            string providerKey,
            IEnumerable<string> grantedPermissions,
            Guid? tenantId = null)
        {
            foreach (var permissionName in grantedPermissions)
            {
                if (await PermissionGrantRepository.FindAsync(permissionName, providerName, providerKey) != null)
                {
                    continue;
                }

                await PermissionGrantRepository.InsertAsync(
                    new PermissionGrant(
                        GuidGenerator.Create(),
                        permissionName,
                        providerName,
                        providerKey,
                        tenantId
                    )
                );
            }
        }
    }
}