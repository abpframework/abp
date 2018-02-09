using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.Permissions
{
    public abstract class PermissionValueProvider : IPermissionValueProvider, ISingletonDependency
    {
        public abstract string Name { get; }

        protected IPermissionStore PermissionStore { get; }

        protected PermissionValueProvider(IPermissionStore permissionStore)
        {
            PermissionStore = permissionStore;
        }

        public abstract Task<bool?> IsGrantedAsync(PermissionDefinition permission, string providerKey);

        public abstract Task SetAsync(PermissionDefinition permission, bool isGranted, string providerKey);

        public abstract Task ClearAsync(PermissionDefinition permission, string providerKey);
    }
}