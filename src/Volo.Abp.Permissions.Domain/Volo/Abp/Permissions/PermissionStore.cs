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

        public async Task<bool> IsGrantedAsync(string name, string providerName, string providerKey)
        {
            return await _permissionGrantRepository.FindAsync(name, providerName, providerKey) != null;
        }

        public async Task AddAsync(string name, string providerName, string providerKey)
        {
            var permissionGrant = await _permissionGrantRepository.FindAsync(name, providerName, providerKey);
            if (permissionGrant != null)
            {
                return;
            }

            await _permissionGrantRepository.InsertAsync(
                new PermissionGrant(GuidGenerator.Create(), name, providerName, providerKey)
            );
        }

        public async Task<List<string>> GetAllGrantedAsync(string providerName, string providerKey)
        {
            var permissionGrants = await _permissionGrantRepository.GetListAsync(providerName, providerKey);
            return permissionGrants.Select(s => s.Name).ToList();
        }

        public async Task RemoveAsync(string name, string providerName, string providerKey)
        {
            var permissionGrant = await _permissionGrantRepository.FindAsync(name, providerName, providerKey);
            if (permissionGrant != null)
            {
                await _permissionGrantRepository.DeleteAsync(permissionGrant);
            }
        }
    }
}
