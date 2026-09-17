using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using OpenIddict.Abstractions;
using Shouldly;
using Volo.Abp.OpenIddict.Applications;
using Volo.Abp.OpenIddict.Authorizations;
using Volo.Abp.OpenIddict.Tokens;
using Volo.Abp.Uow;
using Xunit;

namespace Volo.Abp.OpenIddict.EntityFrameworkCore;

/*
 * OpenIddict tokens/authorizations reference their parent application/authorization via a
 * foreign key. Creating a child (e.g. a token) must NOT bump the parent aggregate root's
 * ConcurrencyStamp, otherwise concurrent creations under a shared parent collide with a
 * DbUpdateConcurrencyException. AbpOpenIddictEntityFrameworkCoreModule excludes both parents
 * from AbpEntityChangeOptions.IgnoredNavigationEntitySelectors. These tests guard that.
 */
public class AbpOpenIddictEntityChangeOptions_Tests : OpenIddictEntityFrameworkCoreTestBase
{
    private readonly AbpOpenIddictTestData _testData;

    public AbpOpenIddictEntityChangeOptions_Tests()
    {
        _testData = ServiceProvider.GetRequiredService<AbpOpenIddictTestData>();
    }

    [Fact]
    public async Task Creating_a_token_should_not_bump_the_referenced_application_stamp()
    {
        var before = await GetApplicationStampAsync(_testData.App1Id);

        await CreateTokenAsync(applicationId: _testData.App1Id, authorizationId: null, subject: "app-stamp-test");

        var after = await GetApplicationStampAsync(_testData.App1Id);
        after.ShouldBe(before);
    }

    [Fact]
    public async Task Creating_a_token_should_not_bump_the_referenced_authorization_stamp()
    {
        var before = await GetAuthorizationStampAsync(_testData.Authorization1Id);

        await CreateTokenAsync(applicationId: _testData.App1Id, authorizationId: _testData.Authorization1Id, subject: "authorization-stamp-test");

        var after = await GetAuthorizationStampAsync(_testData.Authorization1Id);
        after.ShouldBe(before);
    }

    // Loads the referenced parents into the same DbContext (as the OpenIddict flow does) and
    // then creates a token, so the navigation-change detection has a tracked parent to act on.
    private async Task CreateTokenAsync(Guid applicationId, Guid? authorizationId, string subject)
    {
        using var scope = ServiceProvider.CreateScope();
        var uowManager = scope.ServiceProvider.GetRequiredService<IUnitOfWorkManager>();
        using var uow = uowManager.Begin(new AbpUnitOfWorkOptions(), requiresNew: true);

        var applicationRepository = scope.ServiceProvider.GetRequiredService<IOpenIddictApplicationRepository>();
        var authorizationRepository = scope.ServiceProvider.GetRequiredService<IOpenIddictAuthorizationRepository>();
        var tokenStore = scope.ServiceProvider.GetRequiredService<IOpenIddictTokenStore<OpenIddictTokenModel>>();

        await applicationRepository.GetAsync(applicationId);
        if (authorizationId.HasValue)
        {
            await authorizationRepository.GetAsync(authorizationId.Value);
        }

        await tokenStore.CreateAsync(new OpenIddictTokenModel
        {
            Id = Guid.NewGuid(),
            ApplicationId = applicationId,
            AuthorizationId = authorizationId,
            Subject = subject,
            Type = "access_token",
            Status = OpenIddictConstants.Statuses.Valid,
            Payload = "payload-" + subject
        }, CancellationToken.None);

        await uow.CompleteAsync();
    }

    private async Task<string> GetApplicationStampAsync(Guid id)
    {
        using var scope = ServiceProvider.CreateScope();
        var uowManager = scope.ServiceProvider.GetRequiredService<IUnitOfWorkManager>();
        using var uow = uowManager.Begin(new AbpUnitOfWorkOptions(), requiresNew: true);
        var stamp = (await scope.ServiceProvider.GetRequiredService<IOpenIddictApplicationRepository>().GetAsync(id)).ConcurrencyStamp;
        await uow.CompleteAsync();
        return stamp;
    }

    private async Task<string> GetAuthorizationStampAsync(Guid id)
    {
        using var scope = ServiceProvider.CreateScope();
        var uowManager = scope.ServiceProvider.GetRequiredService<IUnitOfWorkManager>();
        using var uow = uowManager.Begin(new AbpUnitOfWorkOptions(), requiresNew: true);
        var stamp = (await scope.ServiceProvider.GetRequiredService<IOpenIddictAuthorizationRepository>().GetAsync(id)).ConcurrencyStamp;
        await uow.CompleteAsync();
        return stamp;
    }
}
