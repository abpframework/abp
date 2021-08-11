using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Guids;
using Volo.Abp.MultiTenancy;

namespace Volo.Abp.PermissionManagement
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
            var multiplePermissionValueProviderGrantInfo = await CheckAsync(new[] {name}, providerName, providerKey);

            return multiplePermissionValueProviderGrantInfo.Result.First().Value;
        }

        public virtual async Task<MultiplePermissionValueProviderGrantInfo> CheckAsync(string[] names, string providerName, string providerKey)
        {
            var multiplePermissionValueProviderGrantInfo = new MultiplePermissionValueProviderGrantInfo(names);
            if (providerName != Name)
            {
                return multiplePermissionValueProviderGrantInfo;
            }

            var permissionGrants = await PermissionGrantRepository.GetListAsync(names, providerName, providerKey);

            foreach (var permissionName in names)
            {
                var isGrant = permissionGrants.Any(x => x.Name == permissionName);
                multiplePermissionValueProviderGrantInfo.Result[permissionName] = new PermissionValueProviderGrantInfo(isGrant, providerKey);
            }

            return multiplePermissionValueProviderGrantInfo;
        }

        public virtual Task SetAsync(string name, string providerKey, bool isGranted)
        {
            return isGranted
                ? GrantAsync(name, providerKey)
                : RevokeAsync(name, providerKey);
        }

        protected virtual async Task GrantAsync(string name, string providerKey)
        {
            var permissionGrant = await PermissionGrantRepository.FindAsync(name, Name, providerKey);
            if (permissionGrant != null)
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

        protected virtual async Task RevokeAsync(string name, string providerKey)
        {
            var permissionGrant = await PermissionGrantRepository.FindAsync(name, Name, providerKey);
            if (permissionGrant == null)
            {
                return;
            }

            await PermissionGrantRepository.DeleteAsync(permissionGrant);
        }
    }
}
