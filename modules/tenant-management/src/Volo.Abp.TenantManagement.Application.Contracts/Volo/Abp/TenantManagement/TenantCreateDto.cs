using System.ComponentModel.DataAnnotations;

namespace Volo.Abp.TenantManagement
{
    public class TenantCreateDto : TenantCreateOrUpdateDtoBase
    {
        [Required]
        [EmailAddress]
        [MaxLength(256)]
        public string AdminEmailAddress { get; set; }

        [Required]
        [MaxLength(128)]
        public string AdminPassword { get; set; }
    }
}