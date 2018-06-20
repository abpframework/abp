namespace Volo.Abp.Identity
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
            public const string RequireConfirmedPhoneNumber = SignInPrefix + ".RequireConfirmedPhoneNumber";
        }
    }
}