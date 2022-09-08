using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using OpenIddict.Abstractions;
using Shouldly;
using Xunit;

namespace Volo.Abp.OpenIddict.Authorizations;

public class AbpOpenIddictAuthorizationStore_Tests : OpenIddictDomainTestBase
{
    private readonly IOpenIddictAuthorizationStore<OpenIddictAuthorizationModel> _authorizationStore;
    private readonly AbpOpenIddictTestData _testData;
    
    public AbpOpenIddictAuthorizationStore_Tests()
    {
        _authorizationStore = ServiceProvider.GetRequiredService<IOpenIddictAuthorizationStore<OpenIddictAuthorizationModel>>();
        _testData = ServiceProvider.GetRequiredService<AbpOpenIddictTestData>();
    }
    
    [Fact]
    public async Task CountAsync()
    {
        var count = await _authorizationStore.CountAsync(CancellationToken.None);
        count.ShouldBe(2);
    }

    [Fact]
    public async Task CreateAsync()
    {
        var id = Guid.NewGuid();
        await _authorizationStore.CreateAsync(new OpenIddictAuthorizationModel {
            Id = id,
            ApplicationId = _testData.App1Id,
            Status = "TestStatus3",
            Subject = "TestSubject3",
            Type = OpenIddictConstants.AuthorizationTypes.Permanent
        }, CancellationToken.None);

        var authorization = await _authorizationStore.FindByIdAsync(id.ToString(), CancellationToken.None);

        authorization.ShouldNotBeNull();
        authorization.Status.ShouldBe("TestStatus3");
        authorization.Subject.ShouldBe("TestSubject3");
        authorization.Type.ShouldBe(OpenIddictConstants.AuthorizationTypes.Permanent);
    }

    [Fact]
    public async Task DeleteAsync()
    {
        var authorization = await _authorizationStore.FindByIdAsync(_testData.Authorization1Id.ToString(), CancellationToken.None);
        await _authorizationStore.DeleteAsync(authorization, CancellationToken.None);
        
        authorization = await _authorizationStore.FindByIdAsync(_testData.Authorization1Id.ToString(), CancellationToken.None);
        authorization.ShouldBeNull();
    }

    [Fact]
    public async Task FindByIdAsync_Should_Return_Null_If_Not_Found()
    {
        var authorization = await _authorizationStore.FindByIdAsync(new Guid().ToString(), CancellationToken.None);
        authorization.ShouldBeNull();
    }
    
    [Fact]
    public async Task FindByIdAsync_Should_Return_Authorization_If_Not_Found()
    {
        var authorization = await _authorizationStore.FindByIdAsync(_testData.Authorization1Id.ToString(), CancellationToken.None);
        authorization.ShouldNotBeNull();
        authorization.Status.ShouldBe("TestStatus1");
        authorization.Subject.ShouldBe("TestSubject1");
        authorization.Type.ShouldBe(OpenIddictConstants.AuthorizationTypes.Permanent);
    }
    
    [Fact]
    public async Task FindByApplicationIdAsync_Should_Return_Empty_If_Not_Found()
    {
        var authorizations = await _authorizationStore.FindByApplicationIdAsync(new Guid().ToString(), CancellationToken.None).ToListAsync();
        
        authorizations.Count.ShouldBe(0);
    }

    [Fact]
    public async Task FindByApplicationIdAsync_Should_Return_Authorizations_If_Found()
    {
        var authorizations = await _authorizationStore.FindByApplicationIdAsync(_testData.App1Id.ToString(), CancellationToken.None).ToListAsync();
        
        authorizations.Count.ShouldBe(1); 
    }
    
    [Fact]
    public async Task FindBySubjectAsync_Should_Return_Empty_If_Not_Found()
    {
        var authorizations = await _authorizationStore.FindBySubjectAsync(new Guid().ToString(), CancellationToken.None).ToListAsync();
        
        authorizations.Count.ShouldBe(0);
    }

    [Fact]
    public async Task FindBySubjectAsync_Should_Return_Authorizations_If_Found()
    {
        var authorizations = await _authorizationStore.FindBySubjectAsync("TestSubject1", CancellationToken.None).ToListAsync();
        
        authorizations.Count.ShouldBe(1); 
    }

    [Fact]
    public async Task UpdateAsync()
    {
        var authorization = await _authorizationStore.FindByIdAsync(_testData.Authorization1Id.ToString(), CancellationToken.None);

        authorization.Status = "New status";
        authorization.Subject = "New subject";
        authorization.Type = OpenIddictConstants.AuthorizationTypes.AdHoc;
        authorization.ApplicationId = _testData.App2Id;

        await _authorizationStore.UpdateAsync(authorization, CancellationToken.None);
        
        authorization = await _authorizationStore.FindByIdAsync(_testData.Authorization1Id.ToString(), CancellationToken.None);
        
        authorization.Status.ShouldBe("New status");
        authorization.Subject.ShouldBe("New subject");
        authorization.Type.ShouldBe(OpenIddictConstants.AuthorizationTypes.AdHoc);
        authorization.ApplicationId.ShouldBe(_testData.App2Id);
    }
}