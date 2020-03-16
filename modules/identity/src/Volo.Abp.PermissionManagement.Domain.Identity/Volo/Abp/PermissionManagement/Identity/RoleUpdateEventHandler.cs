using System.Threading.Tasks;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Entities.Events;
using Volo.Abp.EventBus;
using Volo.Abp.Identity;

namespace Volo.Abp.PermissionManagement.Identity
{
    public class RoleUpdateEventHandler :
        ILocalEventHandler<IdentityRoleNameChangedEvent>,
        ITransientDependency
    {
        protected IIdentityRoleRepository RoleRepository { get; }
        protected IPermissionManager PermissionManager { get; }
        protected IPermissionGrantRepository PermissionGrantRepository { get; }

        public RoleUpdateEventHandler(
            IIdentityRoleRepository roleRepository,
            IPermissionManager permissionManager,
            IPermissionGrantRepository permissionGrantRepository)
        {
            RoleRepository = roleRepository;
            PermissionManager = permissionManager;
            PermissionGrantRepository = permissionGrantRepository;
        }

        public async Task HandleEventAsync(IdentityRoleNameChangedEvent eventData)
        {
            var role = await RoleRepository.FindAsync(eventData.IdentityRole.Id, false);
            if (role == null)
            {
                return;
            }

            var permissionGrantsInRole = await PermissionGrantRepository.GetListAsync(RolePermissionValueProvider.ProviderName, eventData.OldName);
            foreach (var permissionGrant in permissionGrantsInRole)
            {
                await PermissionManager.UpdateProviderKeyAsync(permissionGrant, eventData.IdentityRole.Name);
            }
        }
    }
}
