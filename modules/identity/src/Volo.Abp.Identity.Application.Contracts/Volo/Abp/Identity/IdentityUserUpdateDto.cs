using System.ComponentModel.DataAnnotations;
using Volo.Abp.Auditing;
using Volo.Abp.Domain.Entities;

namespace Volo.Abp.Identity
{
    public class IdentityUserUpdateDto : IdentityUserCreateOrUpdateDtoBase, IHasConcurrencyStamp
    {
        [StringLength(IdentityUserConsts.MaxPasswordLength)]
        [DisableAuditing]
        public string Password { get; set; }
        
        public string ConcurrencyStamp { get; set; }
    }
}