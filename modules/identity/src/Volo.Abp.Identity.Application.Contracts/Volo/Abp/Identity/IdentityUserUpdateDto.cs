using Volo.Abp.Domain.Entities;

namespace Volo.Abp.Identity
{
    public class IdentityUserUpdateDto : IdentityUserCreateOrUpdateDtoBase, IHasConcurrencyStamp
    {
        public string ConcurrencyStamp { get; set; }
    }
}