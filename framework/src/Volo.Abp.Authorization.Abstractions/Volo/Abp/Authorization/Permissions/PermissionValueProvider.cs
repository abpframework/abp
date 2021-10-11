using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.Authorization.Permissions
{
    public abstract class PermissionValueProvider : IPermissionValueProvider, ITransientDependency
    {
        public abstract string Name { get; }

        protected IPermissionStore PermissionStore { get; }

        protected PermissionValueProvider(IPermissionStore permissionStore)
        {
            PermissionStore = permissionStore;
        }

        public abstract Task<PermissionGrantResult> CheckAsync(PermissionValueCheckContext context);

        public abstract Task<MultiplePermissionGrantResult> CheckAsync(PermissionValuesCheckContext context);
    }
}
