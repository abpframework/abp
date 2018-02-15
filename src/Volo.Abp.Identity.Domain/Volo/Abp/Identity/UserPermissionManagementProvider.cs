using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Permissions;

namespace Volo.Abp.Identity
{
    public class UserPermissionManagementProvider : IPermissionManagementProvider, ITransientDependency
    {
        public string Name => "User";

        private readonly IPermissionGrantRepository _permissionGrantRepository;

        public UserPermissionManagementProvider(IPermissionGrantRepository permissionGrantRepository)
        {
            _permissionGrantRepository = permissionGrantRepository;
        }

        public async Task<PermissionValueProviderGrantInfo> CheckAsync(string name, string providerName, string providerKey)
        {
            if (providerName != Name)
            {
                return PermissionValueProviderGrantInfo.NonGranted;
            }

            return new PermissionValueProviderGrantInfo(
                await _permissionGrantRepository.FindAsync(name, providerName, providerKey) != null,
                providerKey
            );
        }
    }
}