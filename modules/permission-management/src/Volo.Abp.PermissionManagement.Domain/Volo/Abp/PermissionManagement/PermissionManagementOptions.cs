using Volo.Abp.Collections;

namespace Volo.Abp.PermissionManagement
{
    public class PermissionManagementOptions
    {
        public ITypeList<IPermissionManagementProvider> ManagementProviders { get; }

        public PermissionManagementOptions()
        {
            ManagementProviders = new TypeList<IPermissionManagementProvider>();
        }
    }
}
