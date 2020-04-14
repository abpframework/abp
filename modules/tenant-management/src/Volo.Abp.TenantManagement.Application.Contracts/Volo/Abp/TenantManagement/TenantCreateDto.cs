using System.ComponentModel.DataAnnotations;

namespace Volo.Abp.TenantManagement
{
    public class TenantCreateDto : TenantCreateOrUpdateDtoBase
    {
        [Required]
        [EmailAddress]
        [MaxLength(256)]
        public virtual string AdminEmailAddress { get; set; }

        [Required]
        [MaxLength(128)]
        public virtual string AdminPassword { get; set; }
    }
}