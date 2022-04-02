using System;
using System.Globalization;
using System.Threading.Tasks;
using OpenIddict.Abstractions;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Guids;
using Volo.Abp.MultiTenancy;
using Volo.Abp.OpenIddict.Applications;
using Volo.Abp.OpenIddict.Scopes;

namespace Volo.Abp.OpenIddict;

public class OpenIddictDataSeedContributor : IDataSeedContributor, ITransientDependency
{
    private readonly IGuidGenerator _guidGenerator;
    private readonly ICurrentTenant _currentTenant;
    private readonly IOpenIddictApplicationManager _applicationManager;
    private readonly IOpenIddictScopeManager _scopeManager;

    public OpenIddictDataSeedContributor(
        IGuidGenerator guidGenerator, ICurrentTenant currentTenant, IOpenIddictApplicationManager applicationManager, IOpenIddictScopeManager scopeManager)
    {
        _guidGenerator = guidGenerator;
        _currentTenant = currentTenant;
        _applicationManager = applicationManager;
        _scopeManager = scopeManager;
    }

    public async Task SeedAsync(DataSeedContext context)
    {
        await CreateScopesAsync();
        await CreateApplicationsAsync();
    }

    private async Task CreateScopesAsync()
    {
        var scope1 = (OpenIddictScope)await _scopeManager.CreateAsync(new OpenIddictScopeDescriptor()
        {
            Name = AbpOpenIddictTestData.Scope1Name,
            DisplayName = "Test Scope 1",
            DisplayNames =
            {
                [CultureInfo.GetCultureInfo("zh-Hans")] = "测试范围1",
                [CultureInfo.GetCultureInfo("tr")] = "Test Kapsamı 1"
            },
            Resources =
            {
                "TestScope1Resource"
            }
        });
        AbpOpenIddictTestData.Scope1Id = scope1.Id;

        var scope2 = (OpenIddictScope)await _scopeManager.CreateAsync(new OpenIddictScopeDescriptor()
        {
            Name = AbpOpenIddictTestData.Scope2Name,
            DisplayName = "Test Scope 2",
            DisplayNames =
            {
                [CultureInfo.GetCultureInfo("zh-Hans")] = "测试范围2",
                [CultureInfo.GetCultureInfo("tr")] = "Test Kapsamı 2"
            },
            Resources =
            {
                "TestScopeResource2"
            }
        });
        AbpOpenIddictTestData.Scope2Id = scope2.Id;
    }


    private async Task CreateApplicationsAsync()
    {
        var app1 = (OpenIddictApplication)await _applicationManager.CreateAsync(new OpenIddictApplicationDescriptor
        {
            ClientId = AbpOpenIddictTestData.App1ClientId,
            ConsentType = OpenIddictConstants.ConsentTypes.Explicit,
            DisplayName = "Test Application",
            RedirectUris =
            {
                new Uri("https://abp.io")
            },
            PostLogoutRedirectUris =
            {
                new Uri("https://abp.io")
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

                OpenIddictConstants.Permissions.Scopes.Roles,
                OpenIddictConstants.Permissions.Scopes.Profile,
                OpenIddictConstants.Permissions.Scopes.Email,
                OpenIddictConstants.Permissions.Scopes.Address,
                OpenIddictConstants.Permissions.Scopes.Phone,

                OpenIddictConstants.Permissions.Prefixes.Scope + AbpOpenIddictTestData.Scope1Name
            }
        });
        AbpOpenIddictTestData.App1Id = app1.Id;

        var app2 = (OpenIddictApplication)await _applicationManager.CreateAsync(new OpenIddictApplicationDescriptor
        {
            ClientId = AbpOpenIddictTestData.App2ClientId,
            ConsentType = OpenIddictConstants.ConsentTypes.Explicit,
            DisplayName = "Test Application",
            RedirectUris =
            {
                new Uri("https://abp.io")
            },
            PostLogoutRedirectUris =
            {
                new Uri("https://abp.io")
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

                OpenIddictConstants.Permissions.Scopes.Roles,
                OpenIddictConstants.Permissions.Scopes.Profile,
                OpenIddictConstants.Permissions.Scopes.Email,
                OpenIddictConstants.Permissions.Scopes.Address,
                OpenIddictConstants.Permissions.Scopes.Phone,

                OpenIddictConstants.Permissions.Prefixes.Scope + AbpOpenIddictTestData.Scope1Name,
                OpenIddictConstants.Permissions.Prefixes.Scope + AbpOpenIddictTestData.Scope2Name,
            }
        });
        AbpOpenIddictTestData.App2Id = app2.Id;
    }
}
