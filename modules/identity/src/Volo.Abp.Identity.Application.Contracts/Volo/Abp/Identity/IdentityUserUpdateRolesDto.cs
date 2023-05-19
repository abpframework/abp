using System.ComponentModel.DataAnnotations;

namespace Volo.Abp.Identity;

public class IdentityUserUpdateRolesDto
{
    [Required]
    public string[] RoleNames { get; set; }
}
