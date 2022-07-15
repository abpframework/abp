using System;
using System.Globalization;
using System.Threading.Tasks;
using OpenIddict.Abstractions;
using OpenIddict.Core;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Guids;
using Volo.Abp.MultiTenancy;
using Volo.Abp.OpenIddict.Applications;
using Volo.Abp.OpenIddict.Authorizations;
using Volo.Abp.OpenIddict.Scopes;
using Volo.Abp.OpenIddict.Tokens;
using Volo.Abp.Timing;

namespace Volo.Abp.OpenIddict;

public class OpenIddictDataSeedContributor : IDataSeedContributor, ITransientDependency
{
    private readonly IOpenIddictApplicationManager _applicationManager;
    private readonly IOpenIddictScopeManager _scopeManager;
    private readonly IOpenIddictTokenManager _tokenManager;
    private readonly IOpenIddictAuthorizationManager _authorizationManager;
    private readonly AbpOpenIddictTestData _testData;
    private readonly IClock _clock;

    public OpenIddictDataSeedContributor(
        IOpenIddictApplicationManager applicationManager, 
        IOpenIddictScopeManager scopeManager,
        IOpenIddictTokenManager tokenManager, 
        IOpenIddictAuthorizationManager authorizationManager,
        IClock clock,
        AbpOpenIddictTestData testData)
    {
        _applicationManager = applicationManager;
        _scopeManager = scopeManager;
        _tokenManager = tokenManager;
        _authorizationManager = authorizationManager;
        _clock = clock;
        _testData = testData;
    }

    public async Task SeedAsync(DataSeedContext context)
    {
        await CreateScopesAsync();
        await CreateApplicationsAsync();
        await CreateAuthorizationsAsync();
        await CreateTokensAsync();
    }

    private async Task CreateScopesAsync()
    {
        await _scopeManager.CreateAsync(await GetOpenIddictScopeModelAsync(_testData.Scope1Id, new OpenIddictScopeDescriptor
        {
            Name = _testData.Scope1Name,
            DisplayName = "Test Scope 1",
            Description = "Test Scope 1",
            DisplayNames =
            {
                [CultureInfo.GetCultureInfo("zh-Hans")] = "测试范围1",
                [CultureInfo.GetCultureInfo("tr")] = "Test Kapsamı 1"
            },
            Resources =
            {
                "TestScope1Resource"
            }
        }));
        
        await _scopeManager.CreateAsync(await GetOpenIddictScopeModelAsync(_testData.Scope2Id, new OpenIddictScopeDescriptor()
        {
            Name = _testData.Scope2Name,
            DisplayName = "Test Scope 2",
            Description = "Test Scope 2",
            DisplayNames =
            {
                [CultureInfo.GetCultureInfo("zh-Hans")] = "测试范围2",
                [CultureInfo.GetCultureInfo("tr")] = "Test Kapsamı 2"
            },
            Resources =
            {
                "TestScopeResource2"
            }
        }));
    }

    private async Task<OpenIddictScopeModel> GetOpenIddictScopeModelAsync(Guid id, OpenIddictScopeDescriptor scopeDescriptor)
    {
        var scope = new OpenIddictScopeModel{Id = id};
        await _scopeManager.PopulateAsync(scope, scopeDescriptor);
        return scope;
    }
    
    private async Task CreateApplicationsAsync()
    {
        await _applicationManager.CreateAsync(await GetOpenIddictApplicationModelAsync(_testData.App1Id, new OpenIddictApplicationDescriptor
        {
            ClientId = _testData.App1ClientId,
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

                OpenIddictConstants.Permissions.Prefixes.Scope + _testData.Scope1Name
            }
        })); 
        
        await _applicationManager.CreateAsync(await GetOpenIddictApplicationModelAsync(_testData.App2Id, new OpenIddictApplicationDescriptor
        {
            ClientId = _testData.App2ClientId,
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

                OpenIddictConstants.Permissions.Prefixes.Scope + _testData.Scope1Name,
                OpenIddictConstants.Permissions.Prefixes.Scope + _testData.Scope2Name,
            }
        }));
    }

    private async Task<OpenIddictApplicationModel> GetOpenIddictApplicationModelAsync(Guid id, OpenIddictApplicationDescriptor applicationDescriptor)
    {
        var application = new OpenIddictApplicationModel{Id = id};
        await _applicationManager.PopulateAsync(application, applicationDescriptor);
        return application;
    }

    private async Task CreateTokensAsync()
    {
        await _tokenManager.CreateAsync(await GetOpenIddictTokenModelAsync(_testData.Token1Id, new OpenIddictTokenDescriptor
        {
            ApplicationId = _testData.App1Id.ToString(),
            AuthorizationId = _testData.Authorization1Id.ToString(),
            Subject = "TestSubject1",
            Type = "TestType1",
            Status = OpenIddictConstants.Statuses.Redeemed,
            Payload = "TestPayload1",
            ReferenceId = "TestReferenceId1",
            ExpirationDate = _clock.Now.AddDays(-30),
            CreationDate = _clock.Now.AddDays(-30)
        }));
        
        await _tokenManager.CreateAsync(await GetOpenIddictTokenModelAsync(_testData.Token2Id, new OpenIddictTokenDescriptor
        {
            ApplicationId = _testData.App2Id.ToString(),
            AuthorizationId = _testData.Authorization2Id.ToString(),
            Subject = "TestSubject2",
            Type = "TestType2",
            Status = OpenIddictConstants.Statuses.Valid,
            Payload = "TestPayload2",
            ReferenceId = "TestReferenceId2",
        }));
    }

    private async Task<OpenIddictTokenModel> GetOpenIddictTokenModelAsync(Guid id, OpenIddictTokenDescriptor tokenDescriptor)
    {
        var token = new OpenIddictTokenModel{Id = id};
        await _tokenManager.PopulateAsync(token, tokenDescriptor);
        return token;
    }

    private async Task CreateAuthorizationsAsync()
    {
        await _authorizationManager.CreateAsync(await GetOpenIddictAuthorizationModelAsync(_testData.Authorization1Id, new OpenIddictAuthorizationDescriptor
        {
            ApplicationId = _testData.App1Id.ToString(),
            Status = "TestStatus1",
            Subject = "TestSubject1",
            Type = OpenIddictConstants.AuthorizationTypes.Permanent,
            CreationDate = _clock.Now.AddDays(-30)
     
        }));
        
        await _authorizationManager.CreateAsync(await GetOpenIddictAuthorizationModelAsync(_testData.Authorization2Id, new OpenIddictAuthorizationDescriptor
        {
            ApplicationId = _testData.App2Id.ToString(),
            Status = "TestStatus2",
            Subject = "TestSubject2",
            Type = OpenIddictConstants.AuthorizationTypes.AdHoc,
            CreationDate = _clock.Now
        }));
    }
    
    private async Task<OpenIddictAuthorizationModel> GetOpenIddictAuthorizationModelAsync(Guid id, OpenIddictAuthorizationDescriptor authorizationDescriptor)
    {
        var authorization = new OpenIddictAuthorizationModel{Id = id};
        await _authorizationManager.PopulateAsync(authorization, authorizationDescriptor);
        return authorization;
    }
}
