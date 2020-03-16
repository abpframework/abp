using System.ComponentModel.DataAnnotations;

namespace Volo.Abp.TenantManagement
{
    public abstract class TenantCreateOrUpdateDtoBase
    {
        [Required]
        [StringLength(TenantConsts.MaxNameLength)]
        public string Name { get; set; }
    }
}