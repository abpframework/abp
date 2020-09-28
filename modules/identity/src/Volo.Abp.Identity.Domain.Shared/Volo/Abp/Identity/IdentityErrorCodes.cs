namespace Volo.Abp.Identity
{
    public static class IdentityErrorCodes
    {
        public const string UserSelfDeletion = "Volo.Abp.Identity:010001";
        public const string MaxAllowedOuMembership = "Volo.Abp.Identity:010002";
        public const string ExternalUserPasswordChange = "Volo.Abp.Identity:010003";
        public const string DuplicateOrganizationUnitDisplayName = "Volo.Abp.Identity:010004";
        public const string StaticRoleRenaming = "Volo.Abp.Identity:010005";
        public const string StaticRoleDeletion = "Volo.Abp.Identity:010006";
        public const string UsersCanNotChangeTwoFactor = "Volo.Abp.Identity:010007";
        public const string CanNotChangeTwoFactor = "Volo.Abp.Identity:010008";
    }
}
