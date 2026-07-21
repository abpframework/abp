using System.Globalization;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.IdentityModel.Tokens;
using OpenIddict.Abstractions;
using OpenIddict.Demo.Server.ExtensionGrants;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.MultiTenancy;

namespace OpenIddict.Demo.Server.EntityFrameworkCore;

public class ServerDataSeedContributor : IDataSeedContributor, ITransientDependency
{
    private readonly ICurrentTenant _currentTenant;
    private readonly IOpenIddictApplicationManager _applicationManager;
    private readonly IOpenIddictScopeManager _scopeManager;

    public ILogger<ServerDataSeedContributor> Logger { get; set; }

    public ServerDataSeedContributor(
        ICurrentTenant currentTenant,
        IOpenIddictApplicationManager applicationManager,
        IOpenIddictScopeManager scopeManager)
    {
        _currentTenant = currentTenant;
        _applicationManager = applicationManager;
        _scopeManager = scopeManager;
        Logger = NullLogger<ServerDataSeedContributor>.Instance;
    }

    public async Task SeedAsync(DataSeedContext context)
    {
        if (await _scopeManager.FindByNameAsync("AbpAPI") == null)
        {
            await _scopeManager.CreateAsync(new OpenIddictScopeDescriptor()
            {
                Name = "AbpAPI",
                DisplayName = "Abp API access",
                DisplayNames =
                {
                    [CultureInfo.GetCultureInfo("zh-Hans")] = "演示 API 访问",
                    [CultureInfo.GetCultureInfo("tr")] = "API erişimi"
                },
                Resources =
                {
                    "AbpAPIResource"
                }
            });
        }

        if (await _applicationManager.FindByClientIdAsync("AbpApp") == null)
        {
            await _applicationManager.CreateAsync(new OpenIddictApplicationDescriptor
            {
                ApplicationType = OpenIddictConstants.ApplicationTypes.Web,
                ClientId = "AbpApp",
                ClientSecret = "1q2w3e*",
                ClientType = OpenIddictConstants.ClientTypes.Confidential,
                ConsentType = OpenIddictConstants.ConsentTypes.Explicit,
                DisplayName = "Abp Application",
                PostLogoutRedirectUris =
                {
                    new Uri("https://localhost:44302/signout-callback-oidc"),
                    new Uri("http://localhost:4200")
                },
                RedirectUris =
                {
                    new Uri("https://localhost:44302/signin-oidc"),
                    new Uri("http://localhost:4200")
                },
                Permissions =
                {
                    OpenIddictConstants.Permissions.Endpoints.Authorization,
                    OpenIddictConstants.Permissions.Endpoints.Token,
                    OpenIddictConstants.Permissions.Endpoints.DeviceAuthorization,
                    OpenIddictConstants.Permissions.Endpoints.Introspection,
                    OpenIddictConstants.Permissions.Endpoints.Revocation,
                    OpenIddictConstants.Permissions.Endpoints.EndSession,
                    OpenIddictConstants.Permissions.Endpoints.PushedAuthorization,

                    OpenIddictConstants.Permissions.GrantTypes.AuthorizationCode,
                    OpenIddictConstants.Permissions.GrantTypes.Implicit,
                    OpenIddictConstants.Permissions.GrantTypes.Password,
                    OpenIddictConstants.Permissions.GrantTypes.RefreshToken,
                    OpenIddictConstants.Permissions.GrantTypes.DeviceCode,
                    OpenIddictConstants.Permissions.GrantTypes.ClientCredentials,
                    OpenIddictConstants.Permissions.GrantTypes.TokenExchange,
                    OpenIddictConstants.Permissions.Prefixes.GrantType + MyTokenExtensionGrant.ExtensionGrantName,

                    OpenIddictConstants.Permissions.ResponseTypes.Code,
                    OpenIddictConstants.Permissions.ResponseTypes.CodeIdToken,
                    OpenIddictConstants.Permissions.ResponseTypes.CodeIdTokenToken,
                    OpenIddictConstants.Permissions.ResponseTypes.CodeToken,
                    OpenIddictConstants.Permissions.ResponseTypes.IdToken,
                    OpenIddictConstants.Permissions.ResponseTypes.IdTokenToken,
                    OpenIddictConstants.Permissions.ResponseTypes.None,
                    OpenIddictConstants.Permissions.ResponseTypes.Token,

                    OpenIddictConstants.Permissions.Scopes.Roles,
                    OpenIddictConstants.Permissions.Scopes.Profile,
                    OpenIddictConstants.Permissions.Scopes.Email,
                    OpenIddictConstants.Permissions.Scopes.Address,
                    OpenIddictConstants.Permissions.Scopes.Phone,
                    OpenIddictConstants.Permissions.Prefixes.Scope + "AbpAPI"
                },
                Settings =
                {
                    // Use a shorter access token lifetime for tokens issued to the Postman application.
                    [OpenIddictConstants.Settings.TokenLifetimes.AccessToken] = TimeSpan.FromMinutes(5).ToString("c", CultureInfo.InvariantCulture)
                }
            });
        }

        if (await _applicationManager.FindByClientIdAsync("AbpBlazorWASMApp") == null)
        {
            await _applicationManager.CreateAsync(new OpenIddictApplicationDescriptor
            {
                ApplicationType = OpenIddictConstants.ApplicationTypes.Web,
                ClientId = "AbpBlazorWASMApp",
                ClientType = OpenIddictConstants.ClientTypes.Public,
                ConsentType = OpenIddictConstants.ConsentTypes.Explicit,
                DisplayName = "Abp Blazor WASM Application",
                PostLogoutRedirectUris =
                {
                    new Uri("https://localhost:44304/authentication/logout-callback")
                },
                RedirectUris =
                {
                    new Uri("https://localhost:44304/authentication/login-callback")
                },
                Permissions =
                {
                    OpenIddictConstants.Permissions.Endpoints.Authorization,
                    OpenIddictConstants.Permissions.Endpoints.Token,
                    OpenIddictConstants.Permissions.Endpoints.DeviceAuthorization,
                    OpenIddictConstants.Permissions.Endpoints.Introspection,
                    OpenIddictConstants.Permissions.Endpoints.Revocation,
                    OpenIddictConstants.Permissions.Endpoints.EndSession,

                    OpenIddictConstants.Permissions.GrantTypes.AuthorizationCode,
                    OpenIddictConstants.Permissions.GrantTypes.Implicit,
                    OpenIddictConstants.Permissions.GrantTypes.Password,
                    OpenIddictConstants.Permissions.GrantTypes.RefreshToken,
                    OpenIddictConstants.Permissions.GrantTypes.DeviceCode,
                    OpenIddictConstants.Permissions.GrantTypes.ClientCredentials,

                    OpenIddictConstants.Permissions.ResponseTypes.Code,
                    OpenIddictConstants.Permissions.ResponseTypes.CodeIdToken,
                    OpenIddictConstants.Permissions.ResponseTypes.CodeIdTokenToken,
                    OpenIddictConstants.Permissions.ResponseTypes.CodeToken,
                    OpenIddictConstants.Permissions.ResponseTypes.IdToken,
                    OpenIddictConstants.Permissions.ResponseTypes.IdTokenToken,
                    OpenIddictConstants.Permissions.ResponseTypes.None,
                    OpenIddictConstants.Permissions.ResponseTypes.Token,

                    OpenIddictConstants.Permissions.Scopes.Roles,
                    OpenIddictConstants.Permissions.Scopes.Profile,
                    OpenIddictConstants.Permissions.Scopes.Email,
                    OpenIddictConstants.Permissions.Scopes.Address,
                    OpenIddictConstants.Permissions.Scopes.Phone,

                    OpenIddictConstants.Permissions.Prefixes.Scope + "AbpAPI"
                }
            });
        }

        if (await _applicationManager.FindByClientIdAsync("AbpConsoleAppWithJwks") == null)
        {
            // Load the pre-generated JWKS (public key) from the jwks.json file.
            // The corresponding private key (jwks-private.pem) is stored in the parent app/ directory
            // and used by OpenIddict.Demo.Client.Console to sign JWT client assertions.
            // Both files are generated with: abp generate-jwks
            var jwksPath = Path.Combine(AppContext.BaseDirectory, "jwks.json");
            if (!File.Exists(jwksPath))
            {
                Logger.LogWarning(
                    "JWKS file not found at '{JwksPath}'. " +
                    "Skipping creation of the 'AbpConsoleAppWithJwks' client. " +
                    "Run 'abp generate-jwks' in the app/ directory to generate the key pair.",
                    jwksPath);
            }
            else
            {
                var jwks = new JsonWebKeySet(await File.ReadAllTextAsync(jwksPath));

                await _applicationManager.CreateAsync(new OpenIddictApplicationDescriptor
                {
                    ApplicationType = OpenIddictConstants.ApplicationTypes.Web,
                    ClientId = "AbpConsoleAppWithJwks",
                    ClientType = OpenIddictConstants.ClientTypes.Confidential,
                    DisplayName = "Abp Console App (private_key_jwt)",
                    JsonWebKeySet = jwks,
                    Permissions =
                    {
                        OpenIddictConstants.Permissions.Endpoints.Token,
                        OpenIddictConstants.Permissions.Endpoints.Introspection,
                        OpenIddictConstants.Permissions.GrantTypes.ClientCredentials,
                        OpenIddictConstants.Permissions.Prefixes.Scope + "AbpAPI"
                    }
                });
            }
        }

        if (await _applicationManager.FindByClientIdAsync("Swagger") == null)
        {
            await _applicationManager.CreateAsync(new OpenIddictApplicationDescriptor
            {
                ApplicationType = OpenIddictConstants.ApplicationTypes.Web,
                ClientId = "Swagger",
                ClientType = OpenIddictConstants.ClientTypes.Public,
                ConsentType = OpenIddictConstants.ConsentTypes.Explicit,
                DisplayName = "Abp Swagger Application",
                RedirectUris =
                {
                    new Uri("https://localhost:44303/swagger/oauth2-redirect.html")
                },
                Permissions =
                {
                    OpenIddictConstants.Permissions.Endpoints.Authorization,
                    OpenIddictConstants.Permissions.Endpoints.Token,

                    OpenIddictConstants.Permissions.GrantTypes.AuthorizationCode,

                    OpenIddictConstants.Permissions.ResponseTypes.Code,

                    OpenIddictConstants.Permissions.Prefixes.Scope + "AbpAPI"
                },
                Settings =
                {
                    // Use a shorter access token lifetime for tokens issued to the Postman application.
                    [OpenIddictConstants.Settings.TokenLifetimes.AccessToken] = TimeSpan.FromMinutes(5).ToString("c", CultureInfo.InvariantCulture)
                }
            });
        }
    }
}
