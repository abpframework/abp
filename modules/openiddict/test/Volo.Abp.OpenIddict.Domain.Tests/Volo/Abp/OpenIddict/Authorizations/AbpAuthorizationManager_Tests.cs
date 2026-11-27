using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using OpenIddict.Abstractions;
using Shouldly;
using Volo.Abp.Uow;
using Xunit;

namespace Volo.Abp.OpenIddict.Authorizations;

public class AbpAuthorizationManager_Tests : OpenIddictDomainTestBase
{
    private readonly IOpenIddictAuthorizationManager _authorizationManager;
    private readonly IUnitOfWorkManager _unitOfWorkManager;
    private readonly AbpOpenIddictTestData _testData;

    public AbpAuthorizationManager_Tests()
    {
        _authorizationManager = ServiceProvider.GetRequiredService<IOpenIddictAuthorizationManager>();
        _unitOfWorkManager = ServiceProvider.GetRequiredService<IUnitOfWorkManager>();
        _testData = ServiceProvider.GetRequiredService<AbpOpenIddictTestData>();
    }

    [Fact]
    public async Task RevokeAsync_Should_Remove_Authorizations_From_Cache()
    {
        (await GetStatusAsync(_testData.Authorization1Id.ToString())).ShouldBe(OpenIddictConstants.Statuses.Valid);

        (await _authorizationManager.RevokeAsync(_testData.Subject1, null, OpenIddictConstants.Statuses.Valid, null)).ShouldBe(1);

        (await GetStatusAsync(_testData.Authorization1Id.ToString())).ShouldBe(OpenIddictConstants.Statuses.Revoked);
    }

    [Fact]
    public async Task RevokeAsync_Should_Remove_Authorizations_From_Cache_When_A_Retry_Changes_No_Rows()
    {
        var authorizationCache = ServiceProvider.GetRequiredService<IOpenIddictAuthorizationCache<OpenIddictAuthorizationModel>>();
        var oldAuthorization = (OpenIddictAuthorizationModel)await _authorizationManager.FindByIdAsync(_testData.Authorization1Id.ToString());

        (await _authorizationManager.RevokeAsync(_testData.Subject1, null, OpenIddictConstants.Statuses.Valid, null)).ShouldBe(1);

        // The cleanup of the first revoke didn't remove the old authorization.
        await authorizationCache.AddAsync(oldAuthorization, CancellationToken.None);

        (await _authorizationManager.RevokeAsync(_testData.Subject1, null, OpenIddictConstants.Statuses.Valid, null)).ShouldBe(0);
        (await GetStatusAsync(_testData.Authorization1Id.ToString())).ShouldBe(OpenIddictConstants.Statuses.Revoked);
    }

    [Fact]
    public async Task RevokeByApplicationIdAsync_Should_Remove_Authorizations_From_Cache()
    {
        (await GetStatusAsync(_testData.Authorization1Id.ToString())).ShouldBe(OpenIddictConstants.Statuses.Valid);

        (await _authorizationManager.RevokeByApplicationIdAsync(_testData.App1Id.ToString())).ShouldBe(1);

        (await GetStatusAsync(_testData.Authorization1Id.ToString())).ShouldBe(OpenIddictConstants.Statuses.Revoked);
    }

    [Fact]
    public async Task RevokeBySubjectAsync_Should_Remove_Authorizations_From_Cache()
    {
        (await GetStatusAsync(_testData.Authorization1Id.ToString())).ShouldBe(OpenIddictConstants.Statuses.Valid);

        (await _authorizationManager.RevokeBySubjectAsync(_testData.Subject1)).ShouldBe(1);

        (await GetStatusAsync(_testData.Authorization1Id.ToString())).ShouldBe(OpenIddictConstants.Statuses.Revoked);
    }

    [Fact]
    public async Task RevokeBySubjectAsync_Should_Remove_Authorizations_From_Cache_In_Non_Transactional_Unit_Of_Work()
    {
        (await GetStatusAsync(_testData.Authorization1Id.ToString())).ShouldBe(OpenIddictConstants.Statuses.Valid);

        using (var uow = _unitOfWorkManager.Begin(new AbpUnitOfWorkOptions { IsTransactional = false }))
        {
            (await _authorizationManager.RevokeBySubjectAsync(_testData.Subject1)).ShouldBe(1);
            (await GetStatusAsync(_testData.Authorization1Id.ToString())).ShouldBe(OpenIddictConstants.Statuses.Revoked);
            await uow.CompleteAsync();
        }
    }

    [Fact]
    public async Task RevokeBySubjectAsync_Should_Remove_Authorizations_From_Cache_When_Transactional_Unit_Of_Work_Is_Completed()
    {
        (await GetStatusAsync(_testData.Authorization1Id.ToString())).ShouldBe(OpenIddictConstants.Statuses.Valid);

        // The test module disables transactions on IUnitOfWorkManager, so the transactional unit of work is started directly.
        using (var uow = ServiceProvider.GetRequiredService<UnitOfWorkManager>().Begin(new AbpUnitOfWorkOptions { IsTransactional = true }))
        {
            (await _authorizationManager.RevokeBySubjectAsync(_testData.Subject1)).ShouldBe(1);
            (await GetStatusAsync(_testData.Authorization1Id.ToString())).ShouldBe(OpenIddictConstants.Statuses.Revoked);
            await uow.CompleteAsync();
        }

        (await GetStatusAsync(_testData.Authorization1Id.ToString())).ShouldBe(OpenIddictConstants.Statuses.Revoked);
    }

    [Fact]
    public async Task FindByIdAsync_In_Rolled_Back_Unit_Of_Work_Should_Not_Cache_Uncommitted_Status()
    {
        using (ServiceProvider.GetRequiredService<UnitOfWorkManager>().Begin(new AbpUnitOfWorkOptions { IsTransactional = true }))
        {
            (await _authorizationManager.RevokeBySubjectAsync(_testData.Subject1)).ShouldBe(1);
            (await GetStatusAsync(_testData.Authorization1Id.ToString())).ShouldBe(OpenIddictConstants.Statuses.Revoked);
        }

        (await GetStatusAsync(_testData.Authorization1Id.ToString())).ShouldBe(OpenIddictConstants.Statuses.Valid);
    }

    [Fact]
    public async Task RevokeBySubjectAsync_Should_Remove_Authorizations_From_Cache_When_Read_In_The_Same_Unit_Of_Work()
    {
        using (var uow = ServiceProvider.GetRequiredService<UnitOfWorkManager>().Begin(new AbpUnitOfWorkOptions { IsTransactional = true }))
        {
            (await GetStatusAsync(_testData.Authorization1Id.ToString())).ShouldBe(OpenIddictConstants.Statuses.Valid);
            (await _authorizationManager.RevokeBySubjectAsync(_testData.Subject1)).ShouldBe(1);
            await uow.CompleteAsync();
        }

        (await GetStatusAsync(_testData.Authorization1Id.ToString())).ShouldBe(OpenIddictConstants.Statuses.Revoked);
    }

    [Fact]
    public async Task UpdateAsync_Should_Remove_Authorization_Cached_Again_Before_Unit_Of_Work_Is_Completed()
    {
        var authorizationCache = ServiceProvider.GetRequiredService<IOpenIddictAuthorizationCache<OpenIddictAuthorizationModel>>();
        var oldAuthorization = (OpenIddictAuthorizationModel)await _authorizationManager.FindByIdAsync(_testData.Authorization1Id.ToString());

        using (var uow = ServiceProvider.GetRequiredService<UnitOfWorkManager>().Begin(new AbpUnitOfWorkOptions { IsTransactional = true }))
        {
            (await _authorizationManager.TryRevokeAsync(await _authorizationManager.FindByIdAsync(_testData.Authorization1Id.ToString()))).ShouldBeTrue();

            // Another request that still reads the committed values caches the old authorization again.
            using (_unitOfWorkManager.Begin(requiresNew: true))
            {
                await authorizationCache.AddAsync(oldAuthorization, CancellationToken.None);
            }

            await uow.CompleteAsync();
        }

        (await GetStatusAsync(_testData.Authorization1Id.ToString())).ShouldBe(OpenIddictConstants.Statuses.Revoked);
    }

    [Fact]
    public async Task DeleteAsync_Should_Remove_Tokens_From_Cache()
    {
        var tokenManager = ServiceProvider.GetRequiredService<IOpenIddictTokenManager>();
        (await tokenManager.FindByIdAsync(_testData.Token1Id.ToString())).ShouldNotBeNull();
        (await tokenManager.FindByIdAsync(_testData.Token2Id.ToString())).ShouldNotBeNull();

        var authorization = await _authorizationManager.FindByIdAsync(_testData.Authorization1Id.ToString());
        await _authorizationManager.DeleteAsync(authorization);

        (await tokenManager.FindByIdAsync(_testData.Token1Id.ToString())).ShouldBeNull();
        (await tokenManager.FindByIdAsync(_testData.Token2Id.ToString())).ShouldBeNull();
    }

    private async Task<string> GetStatusAsync(string id)
    {
        var authorization = await _authorizationManager.FindByIdAsync(id);
        return await _authorizationManager.GetStatusAsync(authorization);
    }
}
