namespace Volo.Abp.Identity
{
    public class IdentityUserCreateOrUpdateDto
    {
        public string UserName { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public bool TwoFactorEnabled { get; set; }

        public bool LockoutEnabled { get; set; }

        public string Password { get; set; }
    }
}