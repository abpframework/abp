using Volo.Abp.Domain.Entities;

namespace Volo.Abp.Identity;

public class IdentityRoleUpdateDto : IdentityRoleCreateOrUpdateDtoBase, IHasConcurrencyStamp
{
    public string ConcurrencyStamp { get; set; }
}
