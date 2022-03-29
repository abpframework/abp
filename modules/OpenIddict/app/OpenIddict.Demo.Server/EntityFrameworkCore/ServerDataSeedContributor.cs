using System.Globalization;
using OpenIddict.Abstractions;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.MultiTenancy;

namespace OpenIddict.Demo.Server.EntityFrameworkCore;

public class ServerDataSeedContributor: IDataSeedContributor, ITransientDependency
{
    private readonly ICurrentTenant _currentTenant;
    private readonly IOpenIddictApplicationManager _applicationManager;
    private readonly IOpenIddictScopeManager _scopeManager;
    
    public ServerDataSeedContributor(
        ICurrentTenant currentTenant, 
        IOpenIddictApplicationManager applicationManager, 
        IOpenIddictScopeManager scopeManager)
    {
        _currentTenant = currentTenant;
        _applicationManager = applicationManager;
        _scopeManager = scopeManager;
    }

    public async Task SeedAsync(DataSeedContext context)
    {
        if (await _scopeManager.FindByNameAsync("OpenIddictDemoAPI") == null)
        {
            await _scopeManager.CreateAsync(new OpenIddictScopeDescriptor() 
            {
                DisplayName = "OpenIddict Demo API access",
                DisplayNames = 
                {
                    [CultureInfo.GetCultureInfo("zh-Hans")] = "演示 OpenIddict API 访问",
                    [CultureInfo.GetCultureInfo("tr")] = "OpenIddict Demo API erişimi"
                },
                Name = "OpenIddictDemoAPI",
                Resources = 
                {
                    "OpenIddictDemoAPIResource"
                }
            });
        }
        
        if (await _applicationManager.FindByClientIdAsync("MyClient") == null)
        {
            await _applicationManager.CreateAsync(new OpenIddictApplicationDescriptor
            {
                ClientId = "MyClient",
                ClientSecret = "901564A5-E7FE-42CB-B10D-61EF6A8F3654",
                ConsentType = OpenIddictConstants.ConsentTypes.Explicit,
                DisplayName = "My Client Application",
                PostLogoutRedirectUris =
                {
                    new Uri("https://localhost:44302/signout-callback-oidc")
                },
                RedirectUris =
                {
                    new Uri("https://localhost:44302/signin-oidc"),
                    new Uri("https://localhost:4200")
                },
                Permissions =
                {
                    OpenIddictConstants.Permissions.Endpoints.Authorization,
                    OpenIddictConstants.Permissions.Endpoints.Token,
                    OpenIddictConstants.Permissions.Endpoints.Device,
                    OpenIddictConstants.Permissions.Endpoints.Introspection,
                    OpenIddictConstants.Permissions.Endpoints.Revocation,
                    OpenIddictConstants.Permissions.Endpoints.Logout,

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
                    
                    OpenIddictConstants.Permissions.Scopes.Email,
                    OpenIddictConstants.Permissions.Scopes.Profile,
                    OpenIddictConstants.Permissions.Scopes.Roles,
                    OpenIddictConstants.Permissions.Scopes.Address,
                    OpenIddictConstants.Permissions.Scopes.Phone,
                    OpenIddictConstants.Permissions.Prefixes.Scope + "OpenIddictDemoAPI"
                }
            });
        }
    }
}
