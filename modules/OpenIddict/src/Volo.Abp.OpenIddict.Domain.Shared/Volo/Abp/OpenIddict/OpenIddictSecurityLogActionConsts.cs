namespace Volo.Abp.OpenIddict;

public class OpenIddictSecurityLogActionConsts
{
    public static string LoginSucceeded { get; set; } = "LoginSucceeded";

    public static string LoginLockedout { get; set; } = "LoginLockedout";

    public static string LoginNotAllowed { get; set; } = "LoginNotAllowed";

    public static string LoginRequiresTwoFactor { get; set; } = "LoginRequiresTwoFactor";

    public static string LoginFailed { get; set; } = "LoginFailed";

    public static string LoginInvalidUserName { get; set; } = "LoginInvalidUserName";

    public static string LoginInvalidUserNameOrPassword { get; set; } = "LoginInvalidUserNameOrPassword";
}