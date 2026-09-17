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
        (await _authorizationRepository.FindAsync(subject: _testData.Subject1, client: new Guid(), status: null, type: null)).Count.ShouldBe(0);
        (await _authorizationRepository.FindAsync(subject: _testData.Subject1, client: _testData.App1Id, status: null, type: null)).Count.ShouldBe(1);
        (await _authorizationRepository.FindAsync(subject: _testData.Subject1, client: _testData.App1Id, status: "NonExistsStatus", type: null)).Count.ShouldBe(0);
        (await _authorizationRepository.FindAsync(subject: _testData.Subject1, client: _testData.App1Id, status: OpenIddictConstants.Statuses.Valid, type: null)).Count.ShouldBe(1);
        (await _authorizationRepository.FindAsync(subject: _testData.Subject1, client: _testData.App1Id, status: OpenIddictConstants.Statuses.Valid ,type: "NonExistsType")).Count.ShouldBe(0);
        (await _authorizationRepository.FindAsync(subject: _testData.Subject1, client: _testData.App1Id, status: OpenIddictConstants.Statuses.Valid ,type: OpenIddictConstants.AuthorizationTypes.Permanent)).Count.ShouldBe(1);
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
        (await _authorizationRepository.FindBySubjectAsync(subject: _testData.Subject1)).Count.ShouldBe(1);
    }

    [Fact]
    public async Task ListAsync()
    {
        (await _authorizationRepository.ListAsync(int.MaxValue, 0)).Count.ShouldBe(2);
        (await _authorizationRepository.ListAsync(int.MaxValue, 2)).Count.ShouldBe(0);
    }

    [Fact]
    public async Task PruneAsync()
    {
        (await _authorizationRepository.ListAsync(int.MaxValue, 0)).Count.ShouldBe(2);
        await _authorizationRepository.PruneAsync(DateTime.UtcNow - TimeSpan.FromDays(14));
        (await _authorizationRepository.ListAsync(int.MaxValue, 0)).Count.ShouldBe(1);
    }

    [Fact]
    public async Task RevokeAsync()
    {
        var authorizations = await _authorizationRepository.FindByApplicationIdAsync(_testData.App1Id);
        authorizations.Count.ShouldBe(1);
        authorizations.First().ApplicationId.ShouldBe(_testData.App1Id);
        authorizations.First().Status.ShouldBe(OpenIddictConstants.Statuses.Valid);

        (await _authorizationRepository.RevokeAsync(null, _testData.App1Id, null, null)).ShouldBe(1);

        authorizations = await _authorizationRepository.FindByApplicationIdAsync(_testData.App1Id);
        authorizations.Count.ShouldBe(1);
        authorizations.First().ApplicationId.ShouldBe(_testData.App1Id);
        authorizations.First().Status.ShouldBe(OpenIddictConstants.Statuses.Revoked);


        authorizations = await _authorizationRepository.FindBySubjectAsync(_testData.Subject2);
        authorizations.Count.ShouldBe(1);
        authorizations.First().Subject.ShouldBe(_testData.Subject2);
        authorizations.First().Status.ShouldBe(OpenIddictConstants.Statuses.Inactive);

        (await _authorizationRepository.RevokeAsync(_testData.Subject2, null, null, null)).ShouldBe(1);

        authorizations = await _authorizationRepository.FindBySubjectAsync(_testData.Subject2);
        authorizations.Count.ShouldBe(1);
        authorizations.First().Subject.ShouldBe(_testData.Subject2);
        authorizations.First().Status.ShouldBe(OpenIddictConstants.Statuses.Revoked);
    }

    [Fact]
    public async Task RevokeByApplicationIdAsync()
    {
        var authorizations = await _authorizationRepository.FindByApplicationIdAsync(_testData.App1Id);
        authorizations.Count.ShouldBe(1);
        authorizations.First().ApplicationId.ShouldBe(_testData.App1Id);
        authorizations.First().Status.ShouldBe(OpenIddictConstants.Statuses.Valid);

        (await _authorizationRepository.RevokeByApplicationIdAsync(_testData.App1Id)).ShouldBe(1);

        authorizations = await _authorizationRepository.FindByApplicationIdAsync(_testData.App1Id);
        authorizations.Count.ShouldBe(1);
        authorizations.First().ApplicationId.ShouldBe(_testData.App1Id);
        authorizations.First().Status.ShouldBe(OpenIddictConstants.Statuses.Revoked);
    }

    [Fact]
    public async Task RevokeBySubjectAsync()
    {
        var authorizations = await _authorizationRepository.FindBySubjectAsync(_testData.Subject1);
        authorizations.Count.ShouldBe(1);
        authorizations.First().Subject.ShouldBe(_testData.Subject1);
        authorizations.First().Status.ShouldBe(OpenIddictConstants.Statuses.Valid);

        (await _authorizationRepository.RevokeBySubjectAsync(_testData.Subject1)).ShouldBe(1);

        authorizations = await _authorizationRepository.FindBySubjectAsync(_testData.Subject1);
        authorizations.Count.ShouldBe(1);
        authorizations.First().Subject.ShouldBe(_testData.Subject1);
        authorizations.First().Status.ShouldBe(OpenIddictConstants.Statuses.Revoked);
    }
}
