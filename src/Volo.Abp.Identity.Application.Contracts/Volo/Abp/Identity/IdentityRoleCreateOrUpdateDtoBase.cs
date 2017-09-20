using System.ComponentModel.DataAnnotations;

namespace Volo.Abp.Identity
{
    public class IdentityRoleCreateOrUpdateDtoBase
    {
        [Required]
        [MaxLength(IdentityRoleConsts.MaxNameLength)]
        public string Name { get; set; }
    }
}