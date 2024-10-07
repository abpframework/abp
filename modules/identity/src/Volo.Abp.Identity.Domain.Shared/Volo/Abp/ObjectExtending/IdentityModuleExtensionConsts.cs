namespace Volo.Abp.ObjectExtending;

public static class IdentityModuleExtensionConsts
{
    public const string ModuleName = "Identity";

    public static class EntityNames
    {
        public const string User = "User";

        public const string Role = "Role";

        public const string ClaimType = "ClaimType";

        public const string OrganizationUnit = "OrganizationUnit";

        public const string IdentitySession = "IdentitySession";
    }

    public static class ConfigurationNames
    {
        public const string AllowUserToEdit = "AllowUserToEdit";
    }
}
