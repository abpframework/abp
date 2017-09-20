namespace Volo.Abp.Identity
{
    public abstract class IdentityUserCreateOrUpdateDtoBase
    {
        public string UserName { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public bool TwoFactorEnabled { get; set; } //TODO: Optional?

        public bool LockoutEnabled { get; set; } //TODO: Optional?
    }
}