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

    /// <summary>
    /// Set the url of the select account page.
    /// </summary>
    public string SelectAccountPage { get; set; } = "~/Account/SelectAccount";

    /// <summary>
    /// When set to <c>true</c>, the access token issued for the <c>client_credentials</c> grant
    /// automatically includes the scopes configured on the client application (permissions
    /// prefixed with <c>oi_scp:</c>) when the client does not explicitly request any scope.
    /// Default: false.
    /// </summary>
    public bool UseDefaultScopesForClientCredentials { get; set; }

    /// <summary>
    /// When set to <c>true</c>, the token response for the <c>password</c> grant automatically
    /// grants the scopes configured on the client application (permissions prefixed with
    /// <c>oi_scp:</c>) when the client does not explicitly request any scope. If the configured
    /// scopes include <c>openid</c>/<c>profile</c>/<c>email</c>/<c>roles</c>, the corresponding
    /// id_token and claim destinations are affected as well.
    /// Default: false.
    /// </summary>
    public bool UseDefaultScopesForPassword { get; set; }

    /// <summary>
    /// When set to <c>true</c>, the token response for the
    /// <c>urn:ietf:params:oauth:grant-type:token-exchange</c> grant automatically grants the
    /// scopes configured on the client application (permissions prefixed with <c>oi_scp:</c>)
    /// when the client does not explicitly request any scope. If the configured scopes include
    /// <c>openid</c>/<c>profile</c>/<c>email</c>/<c>roles</c>, the corresponding id_token and
    /// claim destinations are affected as well.
    /// Default: false.
    /// </summary>
    public bool UseDefaultScopesForTokenExchange { get; set; }
}
