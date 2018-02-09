using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.Permissions
{
    public class PermissionStore : AbpServiceBase, IPermissionStore, ITransientDependency
    {
        private readonly IPermissionGrantRepository _permissionGrantRepository;

        public PermissionStore(IPermissionGrantRepository permissionGrantRepository)
        {
            _permissionGrantRepository = permissionGrantRepository;
        }

        public async Task<bool?> IsGrantedAsync(string name, string providerName, string providerKey)
        {
            var permissionGrant = await _permissionGrantRepository.FindAsync(name, providerName, providerKey);
            return permissionGrant?.IsGranted;
        }

        public async Task SetAsync(string name, bool isGranted, string providerName, string providerKey)
        {
            var permissionGrant = await _permissionGrantRepository.FindAsync(name, providerName, providerKey);
            if (permissionGrant == null)
            {
                permissionGrant = new PermissionGrant(GuidGenerator.Create(), name, isGranted, providerName, providerKey);
                await _permissionGrantRepository.InsertAsync(permissionGrant);
            }
            else
            {
                permissionGrant.IsGranted = isGranted;
                await _permissionGrantRepository.UpdateAsync(permissionGrant);
            }
        }

        public async Task<List<PermissionGrantInfo>> GetListAsync(string providerName, string providerKey)
        {
            var permissionGrants = await _permissionGrantRepository.GetListAsync(providerName, providerKey);
            return permissionGrants.Select(s => new PermissionGrantInfo(s.Name, s.IsGranted)).ToList();
        }

        public async Task DeleteAsync(string name, string providerName, string providerKey)
        {
            var permissionGrant = await _permissionGrantRepository.FindAsync(name, providerName, providerKey);
            if (permissionGrant != null)
            {
                await _permissionGrantRepository.DeleteAsync(permissionGrant);
            }
        }
    }
}
