using System;
using System.Linq;
using System.Threading.Tasks;
using OpenIddict.Abstractions;
using Shouldly;
using Volo.Abp.Modularity;
using Volo.Abp.OpenIddict.Authorizations;
using Xunit;

namespace Volo.Abp.OpenIddict;

public abstract class OpenIddictAuthorizationRepository_Tests<TStartupModule> : OpenIddictTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{
    private readonly IOpenIddictAuthorizationRepository _authorizationRepository;
    private readonly AbpOpenIddictTestData _testData;

    public OpenIddictAuthorizationRepository_Tests()
    {
        _authorizationRepository = GetRequiredService<IOpenIddictAuthorizationRepository>();
        _testData = GetRequiredService<AbpOpenIddictTestData>();
    }
    
    [Fact]
    public async Task FindAsync()
    {
        (await _authorizationRepository.FindAsync(subject:"TestSubject1", client: new Guid())).Count.ShouldBe(0);
        (await _authorizationRepository.FindAsync(subject:"TestSubject1", client: _testData.App1Id)).Count.ShouldBe(1);
        (await _authorizationRepository.FindAsync(subject:"TestSubject1", client: _testData.App1Id, status: "NonExistsStatus")).Count.ShouldBe(0);
        (await _authorizationRepository.FindAsync(subject:"TestSubject1", client: _testData.App1Id, status: "TestStatus1")).Count.ShouldBe(1);
        (await _authorizationRepository.FindAsync(subject:"TestSubject1", client: _testData.App1Id, status: "TestStatus1" ,type: "NonExistsType")).Count.ShouldBe(0);
        (await _authorizationRepository.FindAsync(subject:"TestSubject1", client: _testData.App1Id, status: "TestStatus1" ,type: OpenIddictConstants.AuthorizationTypes.Permanent)).Count.ShouldBe(1);
    }

    [Fact]
    public async Task FindByApplicationIdAsync()
    {
        var authorizations = await _authorizationRepository.FindByApplicationIdAsync(_testData.App1Id);

        authorizations.Count.ShouldBe(1);
        authorizations.First().ApplicationId.ShouldBe(_testData.App1Id);
    }

    [Fact]
    public async Task FindByIdAsync()
    {
        var authorization = await _authorizationRepository.FindByIdAsync(_testData.Authorization1Id);

        authorization.ShouldNotBeNull();
        authorization.Id.ShouldBe(_testData.Authorization1Id);
    }

    [Fact]
    public async Task FindBySubjectAsync()
    {
        (await _authorizationRepository.FindBySubjectAsync(subject:"TestSubject1")).Count.ShouldBe(1);
    }

    [Fact]
    public async Task ListAsync()
    {
        (await _authorizationRepository.ListAsync(int.MaxValue, 0)).Count.ShouldBe(2);
        (await _authorizationRepository.ListAsync(int.MaxValue, 2)).Count.ShouldBe(0);
    }

    [Fact]
    public async Task GetPruneListAsync()
    {
        var threshold = DateTime.UtcNow - TimeSpan.FromDays(14);
        (await _authorizationRepository.GetPruneListAsync(threshold, int.MaxValue)).Count.ShouldBe(1);
    }
}