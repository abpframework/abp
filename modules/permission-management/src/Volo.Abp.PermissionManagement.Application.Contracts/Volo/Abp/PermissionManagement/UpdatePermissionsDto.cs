using Volo.Abp.ObjectExtending;

namespace Volo.Abp.PermissionManagement
{
    public class UpdatePermissionsDto : ExtensibleObject
    {
        public UpdatePermissionDto[] Permissions { get; set; }
    }
}