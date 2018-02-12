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
    }
}
