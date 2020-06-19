using Volo.Abp.ObjectExtending;

namespace Volo.Abp.Identity
{
    public class UserLookupCountInputDto : ExtensibleObject
    {
        public string Filter { get; set; }
    }
}