using Volo.Abp.Security.Claims;

namespace Volo.Abp.OpenIddict;

public class AbpOpenIddictAspNetCoreOptions
{
    /// <summary>
    /// Updates <see cref="AbpClaimTypes"/> to be compatible with OpenIddict claims.
    /// Default: true.
    /// </summary>
    public bool UpdateAbpClaimTypes { get; set; } = true;

    /// <summary>
    /// Set false to suppress AddDeveloperSigningCredential() call on the OpenIddictBuilder.
    /// Default: true.
    /// </summary>
    public bool AddDevelopmentEncryptionAndSigningCertificate { get; set; } = true;

    /// <summary>
    /// Attach auth server current culture info to response.
    /// </summary>
    public bool AttachCultureInfo { get; set; } = true;
}
