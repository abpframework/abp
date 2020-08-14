namespace Volo.Abp.Identity
{
    public class IdentitySecurityLogActionConsts
    {
        public static string LoginSucceeded { get; set; } = "LoginSucceeded";

        public static string LoginLockedout { get; set; } = "LoginLockedout";

        public static string LoginNotAllowed { get; set; } = "LoginNotAllowed";

        public static string LoginRequiresTwoFactor { get; set; } = "LoginRequiresTwoFactor";

        public static string LoginFailed { get; set; } = "LoginFailed";

        public static string LoginInvalidUserName { get; set; } = "LoginInvalidUserName";

        public static string LoginInvalidUserNameOrPassword { get; set; } = "LoginInvalidUserNameOrPassword";

        public static string Logout { get; set; } = "Logout";

        public static string ChangeUserName { get; set; } = "ChangeUserName";

        public static string ChangeEmail { get; set; } = "ChangeEmail";

        public static string ChangePhoneNumber { get; set; } = "ChangePhoneNumber";

        public static string ChangePassword { get; set; } = "ChangePassword";

        public static string TwoFactorEnabled { get; set; } = "TwoFactorEnabled";

        public static string TwoFactorDisabled { get; set; } = "TwoFactorDisabled";
    }
}
