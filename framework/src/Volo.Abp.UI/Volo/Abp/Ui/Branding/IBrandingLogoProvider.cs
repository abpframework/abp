namespace Volo.Abp.Ui.Branding;

/// <summary>
/// Optionally implemented by an <see cref="IBrandingProvider"/> to provide a compact logo,
/// used where the full logo does not fit, like a collapsed menu.
/// </summary>
public interface IBrandingLogoProvider
{
    /// <summary>
    /// Compact logo on white background
    /// </summary>
    string? LogoIconUrl { get; }

    /// <summary>
    /// Compact logo on dark background
    /// </summary>
    string? LogoIconReverseUrl { get; }
}
