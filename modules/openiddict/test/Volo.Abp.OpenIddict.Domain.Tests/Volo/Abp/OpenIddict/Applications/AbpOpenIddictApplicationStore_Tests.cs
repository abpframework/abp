using System;
using System.Collections.Immutable;
using System.Globalization;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using OpenIddict.Abstractions;
using Shouldly;
using Xunit;

namespace Volo.Abp.OpenIddict.Applications;

public class AbpOpenIddictApplicationStore_Tests : OpenIddictDomainTestBase
{
    private readonly IOpenIddictApplicationStore<OpenIddictApplicationModel> _applicationStore;
    private readonly AbpOpenIddictTestData _testData;

    public AbpOpenIddictApplicationStore_Tests()
    {
        _applicationStore = ServiceProvider.GetRequiredService<IOpenIddictApplicationStore<OpenIddictApplicationModel>>();
        _testData = ServiceProvider.GetRequiredService<AbpOpenIddictTestData>();
    }

    [Fact]
    public async Task FindByIdAsync_Should_Return_Null_If_Not_Found()
    {
        var nonExistingId = Guid.NewGuid().ToString();
        var application = await _applicationStore.FindByIdAsync(nonExistingId, CancellationToken.None);
        application.ShouldBeNull();
    }
    
    [Fact]
    public async Task FindByIdAsync_Should_Return_Application_If_Found()
    {
        var application = await _applicationStore.FindByIdAsync(_testData.App1Id.ToString(), CancellationToken.None);
        
        application.ShouldNotBeNull();
        application.ClientId.ShouldBe(_testData.App1ClientId);
        application.ConsentType.ShouldBe(OpenIddictConstants.ConsentTypes.Explicit);
        application.DisplayName.ShouldBe("Test Application");
    }

    [Fact]
    public async Task FindByClientIdAsync_Should_Return_Null_If_Not_Found()
    {
        var nonExistingClientId = Guid.NewGuid().ToString();
        var application = await _applicationStore.FindByClientIdAsync(nonExistingClientId, CancellationToken.None);
        application.ShouldBeNull();
    }

    [Fact]
    public async Task FindByClientIdAsync_Should_Return_Application_If_Found()
    {
        var application = await _applicationStore.FindByClientIdAsync(_testData.App1ClientId, CancellationToken.None);

        application.ShouldNotBeNull();
        application.ClientId.ShouldBe(_testData.App1ClientId);
        application.ConsentType.ShouldBe(OpenIddictConstants.ConsentTypes.Explicit);
        application.DisplayName.ShouldBe("Test Application");
    }

    [Fact]
    public async Task CountAsync()
    {
        var count = await _applicationStore.CountAsync(CancellationToken.None);
        count.ShouldBe(2);
    }

    [Fact]
    public async Task CreateAsync()
    {
        var clientId = Guid.NewGuid().ToString();
        await _applicationStore.CreateAsync(new OpenIddictApplicationModel {
            ClientId = clientId,
            ConsentType = OpenIddictConstants.ConsentTypes.Explicit,
            DisplayName = "Test Application",
            Type = OpenIddictConstants.ClientTypes.Public,
            PostLogoutRedirectUris = "https://abp.io",
            RedirectUris = "https://abp.io"
        }, CancellationToken.None);
        
        var application = await _applicationStore.FindByClientIdAsync(clientId, CancellationToken.None);
        application.ShouldNotBeNull();
        application.ClientId.ShouldBe(clientId);
        application.DisplayName.ShouldBe("Test Application");
        application.Type.ShouldBe(OpenIddictConstants.ClientTypes.Public);
        application.PostLogoutRedirectUris.ShouldBe("https://abp.io");
        application.RedirectUris.ShouldBe("https://abp.io");
    }

    [Fact]
    public async Task DeleteAsync()
    {
        var application = await _applicationStore.FindByIdAsync(_testData.App1Id.ToString(), CancellationToken.None);
        application.ShouldNotBeNull();
        
        await _applicationStore.DeleteAsync(application, CancellationToken.None);
        
        application = await _applicationStore.FindByIdAsync(_testData.App1Id.ToString(), CancellationToken.None);
        application.ShouldBeNull();
    }

    [Fact]
    public async Task FindByPostLogoutRedirectUriAsync_Should_Return_Empty_If_Not_Found()
    {
        var applications = await _applicationStore.FindByPostLogoutRedirectUriAsync("non-existing-uri", CancellationToken.None).ToListAsync();
        applications.Count.ShouldBe(0);
    }

    [Fact]
    public async Task FindByPostLogoutRedirectUriAsync_Should_Return_Applications_If_Found()
    {
        var applications = await _applicationStore.FindByPostLogoutRedirectUriAsync("https://abp.io", CancellationToken.None).ToListAsync();
        applications.Count.ShouldBe(2);
    }
    
    [Fact]
    public async Task FindByRedirectUriAsync_Should_Return_Empty_If_Not_Found()
    {
        var applications = await _applicationStore.FindByRedirectUriAsync("non-existing-uri", CancellationToken.None).ToListAsync();
        applications.Count.ShouldBe(0);
    }
    
    [Fact]
    public async Task FindByRedirectUriAsync_Should_Return_Applications_If_Found()
    {
        var applications = await _applicationStore.FindByRedirectUriAsync("https://abp.io", CancellationToken.None).ToListAsync();
        applications.Count.ShouldBe(2);
    }

    [Fact]
    public async Task GetClientIdAsync()
    {
        var application = await _applicationStore.FindByIdAsync(_testData.App1Id.ToString(), CancellationToken.None);
        var clientId = await _applicationStore.GetClientIdAsync(application, CancellationToken.None);
        
        clientId.ShouldBe(_testData.App1ClientId);
    }

    [Fact]
    public async Task GetClientSecretAsync()
    {
        var application = await _applicationStore.FindByIdAsync(_testData.App1Id.ToString(), CancellationToken.None);
        var secret = await _applicationStore.GetClientIdAsync(application, CancellationToken.None);
        
        secret.ShouldBe("Client1");
    }

    [Fact]
    public async Task GetClientTypeAsync()
    {
        var application = await _applicationStore.FindByIdAsync(_testData.App1Id.ToString(), CancellationToken.None);
        var clientType = await _applicationStore.GetClientTypeAsync(application, CancellationToken.None);
        
        clientType.ShouldBe(OpenIddictConstants.ClientTypes.Public);
    }

    [Fact]
    public async Task GetConsentTypeAsync()
    {
        var application = await _applicationStore.FindByIdAsync(_testData.App1Id.ToString(), CancellationToken.None);
        var consentType = await _applicationStore.GetConsentTypeAsync(application, CancellationToken.None);
        
        consentType.ShouldBe(OpenIddictConstants.ConsentTypes.Explicit);
    }

    [Fact]
    public async Task GetDisplayNameAsync()
    {
        var application = await _applicationStore.FindByIdAsync(_testData.App1Id.ToString(), CancellationToken.None);
        var displayName = await _applicationStore.GetDisplayNameAsync(application, CancellationToken.None);
        
        displayName.ShouldBe("Test Application");
    }

    [Fact]
    public async Task GetIdAsync()
    {
        var application = await _applicationStore.FindByIdAsync(_testData.App1Id.ToString(), CancellationToken.None);
        var id = await _applicationStore.GetIdAsync(application, CancellationToken.None);
        
        id.ShouldBe(_testData.App1Id.ToString());
    }

    [Fact]
    public async Task GetPermissionsAsync()
    {
        var application = await _applicationStore.FindByIdAsync(_testData.App1Id.ToString(), CancellationToken.None);
        var permissions = await _applicationStore.GetPermissionsAsync(application, CancellationToken.None);
        
        permissions.Length.ShouldBeGreaterThan(0);
    }

    [Fact]
    public async Task GetPostLogoutRedirectUrisAsync()
    {
        var application = await _applicationStore.FindByIdAsync(_testData.App1Id.ToString(), CancellationToken.None);
        var postLogoutRedirectUris = await _applicationStore.GetPostLogoutRedirectUrisAsync(application, CancellationToken.None);
        
        postLogoutRedirectUris.Length.ShouldBe(1);
        postLogoutRedirectUris[0].ShouldBe("https://abp.io");
    }

    [Fact]
    public async Task GetRedirectUrisAsync()
    {
        var application = await _applicationStore.FindByIdAsync(_testData.App1Id.ToString(), CancellationToken.None);
        var redirectUris = await _applicationStore.GetRedirectUrisAsync(application, CancellationToken.None);
        
        redirectUris.Length.ShouldBe(1);
        redirectUris[0].ShouldBe("https://abp.io");
    }

    [Fact]
    public async Task GetPropertiesAsync()
    {
        var application = await _applicationStore.FindByIdAsync(_testData.App1Id.ToString(), CancellationToken.None);
        var properties = await _applicationStore.GetPropertiesAsync(application, CancellationToken.None);
        
        properties.Count.ShouldBe(0);
    }

    [Fact]
    public async Task GetRequirementsAsync()
    {
        var application = await _applicationStore.FindByIdAsync(_testData.App1Id.ToString(), CancellationToken.None);
        var requirements = await _applicationStore.GetRequirementsAsync(application, CancellationToken.None);
        
        requirements.Length.ShouldBe(0);
    }

    [Fact]
    public async Task InstantiateAsync()
    {
        var application = await _applicationStore.InstantiateAsync(CancellationToken.None); 
        application.ShouldNotBeNull();
    }

    [Fact]
    public async Task ListAsync_Should_Return_Empty_If_Not_Found()
    {
        var applications = await _applicationStore.ListAsync(2,2, CancellationToken.None).ToListAsync();
        applications.Count.ShouldBe(0);
    }

    [Fact]
    public async Task ListAsync_Should_Return_Applications_If_Found()
    {
        var applications = await _applicationStore.ListAsync(2,0, CancellationToken.None).ToListAsync();
        applications.Count.ShouldBe(2);
    }

    [Fact]
    public async Task SetClientIdAsync()
    {
        var clientId = Guid.NewGuid().ToString();
        var application = await _applicationStore.FindByIdAsync(_testData.App1Id.ToString(), CancellationToken.None);
        await _applicationStore.SetClientIdAsync(application, clientId, CancellationToken.None);

        application.ClientId.ShouldBe(clientId);
    }

    [Fact]
    public async Task SetClientSecretAsync()
    {
        var clientSecret = Guid.NewGuid().ToString();
        var application = await _applicationStore.FindByIdAsync(_testData.App1Id.ToString(), CancellationToken.None);
        await _applicationStore.SetClientSecretAsync(application, clientSecret, CancellationToken.None);
        
        application.ClientSecret.ShouldBe(clientSecret);
    }

    [Fact]
    public async Task SetClientTypeAsync()
    {
        var application = await _applicationStore.FindByIdAsync(_testData.App1Id.ToString(), CancellationToken.None);
        await _applicationStore.SetClientTypeAsync(application, OpenIddictConstants.ClientTypes.Confidential, CancellationToken.None);
        
        application.Type.ShouldBe(OpenIddictConstants.ClientTypes.Confidential);
    }

    [Fact]
    public async Task SetConsentTypeAsync()
    {
        var application = await _applicationStore.FindByIdAsync(_testData.App1Id.ToString(), CancellationToken.None);
        await _applicationStore.SetConsentTypeAsync(application, OpenIddictConstants.ConsentTypes.Systematic, CancellationToken.None);
        
        application.ConsentType.ShouldBe(OpenIddictConstants.ConsentTypes.Systematic);
    }
    
    [Fact]
    public async Task SetDisplayNameAsync()
    {
        var displayName = Guid.NewGuid().ToString();
        var application = await _applicationStore.FindByIdAsync(_testData.App1Id.ToString(), CancellationToken.None);
        await _applicationStore.SetDisplayNameAsync(application, displayName, CancellationToken.None);
        
        application.DisplayName.ShouldBe(displayName);
    }

    [Fact]
    public async Task SetDisplayNamesAsync()
    {
        var displayNames = ImmutableDictionary.Create<CultureInfo, string>();
        displayNames = displayNames.Add(CultureInfo.GetCultureInfo("en"), "Test Application");
        displayNames = displayNames.Add(CultureInfo.GetCultureInfo("zh-Hans"), "测试应用程序");
        
        var application = await _applicationStore.FindByIdAsync(_testData.App1Id.ToString(), CancellationToken.None);
        await _applicationStore.SetDisplayNamesAsync(application, displayNames, CancellationToken.None);

        application.DisplayNames.ShouldContain("Test Application");
        application.DisplayNames.ShouldContain("测试应用程序");
    }

    [Fact]
    public async Task SetPermissionsAsync()
    {
        var application = await _applicationStore.FindByIdAsync(_testData.App1Id.ToString(), CancellationToken.None);
        await _applicationStore.SetPermissionsAsync(application, ImmutableArray.Create(OpenIddictConstants.Permissions.Endpoints.Authorization), CancellationToken.None);
        
        application.Permissions.ShouldBe("[\""+OpenIddictConstants.Permissions.Endpoints.Authorization+"\"]");
    }

    [Fact]
    public async Task SetPostLogoutRedirectUrisAsync()
    {
        var application = await _applicationStore.FindByIdAsync(_testData.App1Id.ToString(), CancellationToken.None);
        await _applicationStore.SetPostLogoutRedirectUrisAsync(application, ImmutableArray.Create("https://abp.io"), CancellationToken.None);
        
        application.PostLogoutRedirectUris.ShouldBe("[\"https://abp.io\"]");
    }

    [Fact]
    public async Task SetPropertiesAsync()
    {
        var application = await _applicationStore.FindByIdAsync(_testData.App1Id.ToString(), CancellationToken.None);
        await _applicationStore.SetPropertiesAsync(application, ImmutableDictionary.Create<string, JsonElement>(), CancellationToken.None);
        
        application.Properties.ShouldBeNull();
    }

    [Fact]
    public async Task SetRedirectUrisAsync()
    {
        var application = await _applicationStore.FindByIdAsync(_testData.App1Id.ToString(), CancellationToken.None);
        await _applicationStore.SetRedirectUrisAsync(application, ImmutableArray.Create("https://abp.io"), CancellationToken.None);
        
        application.RedirectUris.ShouldBe("[\"https://abp.io\"]");
    }

    [Fact]
    public async Task SetRequirementsAsync()
    {
        var application = await _applicationStore.FindByIdAsync(_testData.App1Id.ToString(), CancellationToken.None);
        await _applicationStore.SetRequirementsAsync(application, ImmutableArray.Create(OpenIddictConstants.Requirements.Features.ProofKeyForCodeExchange), CancellationToken.None);
        
        application.Requirements.ShouldBe("[\""+OpenIddictConstants.Requirements.Features.ProofKeyForCodeExchange+"\"]");
    }

    [Fact]
    public async Task UpdateAsync()
    {
        var application = await _applicationStore.FindByIdAsync(_testData.App1Id.ToString(), CancellationToken.None);
        application.ClientId = "new_client_id";
        application.Type = OpenIddictConstants.ClientTypes.Public;
        application.RedirectUris = "https://new_logout_uri";
        application.PostLogoutRedirectUris = "https://new_post_logout_uri";
        application.DisplayName = "new_display_name";
        
        await _applicationStore.UpdateAsync(application, CancellationToken.None);
        application = await _applicationStore.FindByIdAsync(_testData.App1Id.ToString(), CancellationToken.None);

        application.ShouldNotBeNull();
        application.ClientId.ShouldBe("new_client_id");
        application.Type.ShouldBe(OpenIddictConstants.ClientTypes.Public);
        application.RedirectUris.ShouldBe("https://new_logout_uri");
        application.PostLogoutRedirectUris.ShouldBe("https://new_post_logout_uri");
        application.DisplayName.ShouldBe("new_display_name");
    }
}