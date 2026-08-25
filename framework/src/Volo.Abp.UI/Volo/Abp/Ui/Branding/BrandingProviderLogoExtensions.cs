namespace Volo.Abp.Ui.Branding;

public static class BrandingProviderLogoExtensions
{
    /// <summary>
    /// Returns the compact logo of the branding provider on white background when available; otherwise, null.
    /// </summary>
    public static string? GetLogoIconUrlOrNull(this IBrandingProvider brandingProvider)
    {
        return (brandingProvider as IBrandingLogoProvider)?.LogoIconUrl;
    }

    /// <summary>
    /// Returns the compact logo of the branding provider on dark background when available; otherwise, null.
    /// </summary>
    public static string? GetLogoIconReverseUrlOrNull(this IBrandingProvider brandingProvider)
    {
        return (brandingProvider as IBrandingLogoProvider)?.LogoIconReverseUrl;
    }
}
