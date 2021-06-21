namespace Volo.Abp.Identity.Settings
{
    public static class IdentitySettingNames
    {
        private const string Prefix = "Abp.Identity";

        public static class Password
        {
            private const string PasswordPrefix = Prefix + ".Password";

            public const string RequiredLength = PasswordPrefix + ".RequiredLength";
            public const string RequiredUniqueChars = PasswordPrefix + ".RequiredUniqueChars";
            public const string RequireNonAlphanumeric = PasswordPrefix + ".RequireNonAlphanumeric";
            public const string RequireLowercase = PasswordPrefix + ".RequireLowercase";
            public const string RequireUppercase = PasswordPrefix + ".RequireUppercase";
            public const string RequireDigit = PasswordPrefix + ".RequireDigit";
        }

        public static class Lockout
        {
            private const string LockoutPrefix = Prefix + ".Lockout";

            public const string AllowedForNewUsers = LockoutPrefix + ".AllowedForNewUsers";
            public const string LockoutDuration = LockoutPrefix + ".LockoutDuration";
            public const string MaxFailedAccessAttempts = LockoutPrefix + ".MaxFailedAccessAttempts";
        }

        public static class SignIn
        {
            private const string SignInPrefix = Prefix + ".SignIn";

            public const string RequireConfirmedEmail = SignInPrefix + ".RequireConfirmedEmail";
            public const string EnablePhoneNumberConfirmation = SignInPrefix + ".EnablePhoneNumberConfirmation";
            public const string RequireConfirmedPhoneNumber = SignInPrefix + ".RequireConfirmedPhoneNumber";
        }

        public static class User
        {
            private const string UserPrefix = Prefix + ".User";

            public const string IsUserNameUpdateEnabled = UserPrefix + ".IsUserNameUpdateEnabled";
            public const string IsEmailUpdateEnabled = UserPrefix + ".IsEmailUpdateEnabled";
        }

        public static class OrganizationUnit
        {
            private const string OrganizationUnitPrefix = Prefix + ".OrganizationUnit";

            public const string MaxUserMembershipCount = OrganizationUnitPrefix + ".MaxUserMembershipCount";
        }
    }
}
