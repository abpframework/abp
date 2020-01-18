using System.ComponentModel.DataAnnotations;
using Volo.Abp.Domain.Entities;

namespace Volo.Abp.Identity
{
    public class IdentityUserUpdateDto : IdentityUserCreateOrUpdateDtoBase, IHasConcurrencyStamp
    {
        [StringLength(IdentityUserConsts.MaxPasswordLength)]
        public string Password { get; set; }
        
        public string ConcurrencyStamp { get; set; }
    }
}