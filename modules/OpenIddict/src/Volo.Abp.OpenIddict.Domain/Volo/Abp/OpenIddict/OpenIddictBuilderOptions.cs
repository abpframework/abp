using Volo.Abp.Security.Claims;

namespace Volo.Abp.OpenIddict;

public class OpenIddictBuilderOptions
{
    /// <summary>
    /// Updates <see cref="AbpClaimTypes"/> to be compatible with identity server claims.
    /// Default: true.
    /// </summary>
    public bool UpdateAbpClaimTypes { get; set; } = true;

    /// <summary>
    /// Set false to suppress AddDeveloperSigningCredential() call on the IIdentityServerBuilder.
    /// </summary>
    public bool AddDevelopmentEncryptionAndSigningCertificate { get; set; } = true;
}
