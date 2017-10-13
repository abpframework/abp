namespace Volo.Abp.Identity.Web.Areas.Identity.Models
{
    public class CreateOrUpdateUserViewModel
    {
        public IdentityUserDto User { get; set; }
        public IdentityUserRoleDto[] Roles { get; set; }
    }
}
