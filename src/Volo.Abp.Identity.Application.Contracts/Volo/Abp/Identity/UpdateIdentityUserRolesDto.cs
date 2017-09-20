using System.ComponentModel.DataAnnotations;

namespace Volo.Abp.Identity
{
    public class UpdateIdentityUserRolesDto
    {
        [Required]
        public string[] RoleNames { get; set; }
    }
}