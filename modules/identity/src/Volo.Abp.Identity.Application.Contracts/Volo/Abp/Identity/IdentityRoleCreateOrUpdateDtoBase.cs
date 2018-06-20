using System.ComponentModel.DataAnnotations;

namespace Volo.Abp.Identity
{
    public class IdentityRoleCreateOrUpdateDtoBase
    {
        [Required]
        [StringLength(IdentityRoleConsts.MaxNameLength)]
        public string Name { get; set; }
    }
}