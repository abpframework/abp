using Volo.Abp.ObjectExtending;

namespace Volo.Abp.Identity
{
    public class ChangePasswordInput : ExtensibleObject
    {
        public string CurrentPassword { get; set; }

        public string NewPassword { get; set; }
    }
}
