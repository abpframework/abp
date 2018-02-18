using System.Threading.Tasks;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Guids;
using Volo.Abp.MultiTenancy;

namespace Volo.Abp.Permissions
{
    public abstract class PermissionManagementProvider : IPermissionManagementProvider
    {
        public abstract string Name { get; }

        protected IPermissionGrantRepository PermissionGrantRepository { get; }

        protected IGuidGenerator GuidGenerator { get; }

        protected ICurrentTenant CurrentTenant { get; }

        protected PermissionManagementProvider(
            IPermissionGrantRepository permissionGrantRepository, 
            IGuidGenerator guidGenerator,
            ICurrentTenant currentTenant)
        {
            PermissionGrantRepository = permissionGrantRepository;
            GuidGenerator = guidGenerator;
            CurrentTenant = currentTenant;
        }


        public virtual async Task<PermissionValueProviderGrantInfo> CheckAsync(string name, string providerName, string providerKey)
        {
            if (providerName != Name)
            {
                return PermissionValueProviderGrantInfo.NonGranted;
            }

            return new PermissionValueProviderGrantInfo(
                await PermissionGrantRepository.FindAsync(name, providerName, providerKey) != null,
                providerKey
            );
        }

        public virtual async Task GrantAsync(string name, string providerKey)
        {
            var grant = await PermissionGrantRepository.FindAsync(name, Name, providerKey);
            if (grant != null)
            {
                return;
            }

            await PermissionGrantRepository.InsertAsync(
                new PermissionGrant(
                    GuidGenerator.Create(),
                    name,
                    Name,
                    providerKey,
                    CurrentTenant.Id
                )
            );
        }

        public virtual async Task RevokeAsync(string name, string providerKey)
        {
            var grant = await PermissionGrantRepository.FindAsync(name, Name, providerKey);
            if (grant == null)
            {
                return;
            }

            await PermissionGrantRepository.DeleteAsync(grant);
        }
    }
}