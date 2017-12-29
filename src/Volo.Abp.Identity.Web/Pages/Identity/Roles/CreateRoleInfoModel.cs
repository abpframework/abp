using System.ComponentModel.DataAnnotations;

namespace Volo.Abp.Identity.Web.Pages.Identity.Roles
{
    public class CreateRoleInfoModel
    {
        [Required]
        [StringLength(IdentityRoleConsts.MaxNameLength)]
        [Display(Name = "RoleName")]
        public string Name { get; set; }
    }
}