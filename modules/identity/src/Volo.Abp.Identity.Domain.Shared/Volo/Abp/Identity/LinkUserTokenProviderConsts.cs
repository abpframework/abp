namespace Volo.Abp.Identity;

public static class LinkUserTokenProviderConsts
{
    public static string LinkUserTokenProviderName { get; set; } = "AbpLinkUser";

    public static string LinkUserTokenPurpose { get; set; } = "AbpLinkUser";

    public static string LinkUserLoginTokenPurpose { get; set; } = "AbpLinkUserLogin";

    public static string LinkUserConsentLoginProvider { get; set; } = "[AbpLinkUserConsent]";

    public static string LinkUserConsentTokenName { get; set; } = "Consent";
}
