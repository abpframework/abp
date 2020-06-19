using Volo.Abp.ObjectExtending;

namespace Volo.Abp.PermissionManagement
{
    public class UpdatePermissionDto : ExtensibleObject
    {
        public string Name { get; set; }

        public bool IsGranted { get; set; }
    }
}