using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using OpenIddict.Abstractions;
using Shouldly;
using Volo.Abp.Uow;
using Xunit;

namespace Volo.Abp.OpenIddict.Tokens;

public class AbpTokenManager_Tests : OpenIddictDomainTestBase
{
    private readonly IOpenIddictTokenManager _tokenManager;
    private readonly IUnitOfWorkManager _unitOfWorkManager;
    private readonly AbpOpenIddictTestData _testData;

    public AbpTokenManager_Tests()
    {
        _tokenManager = ServiceProvider.GetRequiredService<IOpenIddictTokenManager>();
        _unitOfWorkManager = ServiceProvider.GetRequiredService<IUnitOfWorkManager>();
        _testData = ServiceProvider.GetRequiredService<AbpOpenIddictTestData>();
    }

    [Fact]
    public async Task RevokeAsync_Should_Remove_Tokens_From_Cache()
    {
        (await GetStatusAsync(_testData.Token2Id.ToString())).ShouldBe(OpenIddictConstants.Statuses.Valid);

        (await _tokenManager.RevokeAsync(_testData.Subject2, null, OpenIddictConstants.Statuses.Valid, null)).ShouldBe(1);

        (await GetStatusAsync(_testData.Token2Id.ToString())).ShouldBe(OpenIddictConstants.Statuses.Revoked);
    }

    [Fact]
    public async Task RevokeByApplicationIdAsync_Should_Remove_Tokens_From_Cache()
    {
        (await GetStatusAsync(_testData.Token2Id.ToString())).ShouldBe(OpenIddictConstants.Statuses.Valid);

        (await _tokenManager.RevokeByApplicationIdAsync(_testData.App2Id.ToString())).ShouldBe(1);

        (await GetStatusAsync(_testData.Token2Id.ToString())).ShouldBe(OpenIddictConstants.Statuses.Revoked);
    }

    [Fact]
    public async Task RevokeByAuthorizationIdAsync_Should_Remove_Tokens_From_Cache()
    {
        (await GetStatusAsync(_testData.Token1Id.ToString())).ShouldBe(OpenIddictConstants.Statuses.Redeemed);
        (await GetStatusAsync(_testData.Token2Id.ToString())).ShouldBe(OpenIddictConstants.Statuses.Valid);

        (await _tokenManager.RevokeByAuthorizationIdAsync(_testData.Authorization1Id.ToString())).ShouldBe(2);

        (await GetStatusAsync(_testData.Token1Id.ToString())).ShouldBe(OpenIddictConstants.Statuses.Revoked);
        (await GetStatusAsync(_testData.Token2Id.ToString())).ShouldBe(OpenIddictConstants.Statuses.Revoked);
    }

    [Fact]
    public async Task RevokeBySubjectAsync_Should_Remove_Tokens_From_Cache()
    {
        (await GetStatusAsync(_testData.Token2Id.ToString())).ShouldBe(OpenIddictConstants.Statuses.Valid);

        (await _tokenManager.RevokeBySubjectAsync(_testData.Subject2)).ShouldBe(1);

        (await GetStatusAsync(_testData.Token2Id.ToString())).ShouldBe(OpenIddictConstants.Statuses.Revoked);
    }

    [Fact]
    public async Task RevokeByAuthorizationIdAsync_Should_Remove_Tokens_From_Cache_In_Non_Transactional_Unit_Of_Work()
    {
        (await GetStatusAsync(_testData.Token2Id.ToString())).ShouldBe(OpenIddictConstants.Statuses.Valid);

        using (var uow = _unitOfWorkManager.Begin(new AbpUnitOfWorkOptions { IsTransactional = false }))
        {
            (await _tokenManager.RevokeByAuthorizationIdAsync(_testData.Authorization1Id.ToString())).ShouldBe(2);
            (await GetStatusAsync(_testData.Token2Id.ToString())).ShouldBe(OpenIddictConstants.Statuses.Revoked);
            await uow.CompleteAsync();
        }
    }

    [Fact]
    public async Task RevokeByAuthorizationIdAsync_Should_Remove_Tokens_From_Cache_When_Transactional_Unit_Of_Work_Is_Completed()
    {
        (await GetStatusAsync(_testData.Token2Id.ToString())).ShouldBe(OpenIddictConstants.Statuses.Valid);

        // The test module disables transactions on IUnitOfWorkManager, so the transactional unit of work is started directly.
        using (var uow = ServiceProvider.GetRequiredService<UnitOfWorkManager>().Begin(new AbpUnitOfWorkOptions { IsTransactional = true }))
        {
            (await _tokenManager.RevokeByAuthorizationIdAsync(_testData.Authorization1Id.ToString())).ShouldBe(2);
            (await GetStatusAsync(_testData.Token2Id.ToString())).ShouldBe(OpenIddictConstants.Statuses.Revoked);
            await uow.CompleteAsync();
        }

        (await GetStatusAsync(_testData.Token2Id.ToString())).ShouldBe(OpenIddictConstants.Statuses.Revoked);
    }

    [Theory]
    [InlineData("id")]
    [InlineData("reference")]
    [InlineData("subject")]
    public async Task Lookup_In_Rolled_Back_Unit_Of_Work_Should_Not_Cache_Uncommitted_Status(string lookup)
    {
        var tokenCache = ServiceProvider.GetRequiredService<IOpenIddictTokenCache<OpenIddictTokenModel>>();
        var referenceId = (await ServiceProvider.GetRequiredService<IOpenIddictTokenRepository>().FindByIdAsync(_testData.Token2Id)).ReferenceId;

        using (ServiceProvider.GetRequiredService<UnitOfWorkManager>().Begin(new AbpUnitOfWorkOptions { IsTransactional = true }))
        {
            (await _tokenManager.RevokeBySubjectAsync(_testData.Subject2)).ShouldBe(1);

            var status = lookup switch
            {
                "id" => await GetStatusAsync(_testData.Token2Id.ToString()),
                "reference" => (await tokenCache.FindByReferenceIdAsync(referenceId, CancellationToken.None)).Status,
                _ => (await GetStatusesAsync(_tokenManager.FindBySubjectAsync(_testData.Subject2))).Single()
            };
            status.ShouldBe(OpenIddictConstants.Statuses.Revoked);
        }

        (await tokenCache.FindByReferenceIdAsync(referenceId, CancellationToken.None)).Status.ShouldBe(OpenIddictConstants.Statuses.Valid);
        (await GetStatusAsync(_testData.Token2Id.ToString())).ShouldBe(OpenIddictConstants.Statuses.Valid);
        (await GetStatusesAsync(_tokenManager.FindBySubjectAsync(_testData.Subject2))).ShouldBe(new[] { OpenIddictConstants.Statuses.Valid });
    }

    [Fact]
    public async Task RevokeBySubjectAsync_Should_Remove_Tokens_From_Cache_When_Read_In_The_Same_Unit_Of_Work()
    {
        var tokenCache = ServiceProvider.GetRequiredService<IOpenIddictTokenCache<OpenIddictTokenModel>>();
        var referenceId = (await ServiceProvider.GetRequiredService<IOpenIddictTokenRepository>().FindByIdAsync(_testData.Token2Id)).ReferenceId;

        using (var uow = ServiceProvider.GetRequiredService<UnitOfWorkManager>().Begin(new AbpUnitOfWorkOptions { IsTransactional = true }))
        {
            (await GetStatusAsync(_testData.Token2Id.ToString())).ShouldBe(OpenIddictConstants.Statuses.Valid);
            (await _tokenManager.RevokeBySubjectAsync(_testData.Subject2)).ShouldBe(1);
            await uow.CompleteAsync();
        }

        (await tokenCache.FindByReferenceIdAsync(referenceId, CancellationToken.None)).Status.ShouldBe(OpenIddictConstants.Statuses.Revoked);
        (await GetStatusAsync(_testData.Token2Id.ToString())).ShouldBe(OpenIddictConstants.Statuses.Revoked);
        (await GetStatusesAsync(_tokenManager.FindBySubjectAsync(_testData.Subject2))).ShouldBe(new[] { OpenIddictConstants.Statuses.Revoked });
    }

    [Fact]
    public async Task UpdateAsync_Should_Remove_Token_Cached_Again_Before_Unit_Of_Work_Is_Completed()
    {
        var tokenCache = ServiceProvider.GetRequiredService<IOpenIddictTokenCache<OpenIddictTokenModel>>();
        var oldToken = (OpenIddictTokenModel)await _tokenManager.FindByIdAsync(_testData.Token2Id.ToString());

        using (var uow = ServiceProvider.GetRequiredService<UnitOfWorkManager>().Begin(new AbpUnitOfWorkOptions { IsTransactional = true }))
        {
            (await _tokenManager.TryRevokeAsync(await _tokenManager.FindByIdAsync(_testData.Token2Id.ToString()))).ShouldBeTrue();

            // Another request that still reads the committed values caches the old token again.
            using (_unitOfWorkManager.Begin(requiresNew: true))
            {
                await tokenCache.AddAsync(oldToken, CancellationToken.None);
            }

            await uow.CompleteAsync();
        }

        // A miss on one key caches the token under both keys, so the reference id is read first.
        (await tokenCache.FindByReferenceIdAsync(oldToken.ReferenceId, CancellationToken.None)).Status.ShouldBe(OpenIddictConstants.Statuses.Revoked);
        (await GetStatusAsync(_testData.Token2Id.ToString())).ShouldBe(OpenIddictConstants.Statuses.Revoked);
    }

    [Fact]
    public async Task RevokeByAuthorizationIdAsync_Should_Remove_Tokens_From_Reference_Id_Cache()
    {
        var tokenCache = ServiceProvider.GetRequiredService<IOpenIddictTokenCache<OpenIddictTokenModel>>();
        var referenceId = (await ServiceProvider.GetRequiredService<IOpenIddictTokenRepository>().FindByIdAsync(_testData.Token2Id)).ReferenceId;
        (await tokenCache.FindByReferenceIdAsync(referenceId, CancellationToken.None)).Status.ShouldBe(OpenIddictConstants.Statuses.Valid);

        (await _tokenManager.RevokeByAuthorizationIdAsync(_testData.Authorization1Id.ToString())).ShouldBe(2);

        (await tokenCache.FindByReferenceIdAsync(referenceId, CancellationToken.None)).Status.ShouldBe(OpenIddictConstants.Statuses.Revoked);
    }

    [Fact]
    public async Task RevokeBySubjectAsync_Should_Not_Leave_A_Stale_Filtered_List()
    {
        (await GetStatusesAsync(_tokenManager.FindAsync(_testData.Subject2, null, null, null))).ShouldBe(new[] { OpenIddictConstants.Statuses.Valid });

        (await _tokenManager.RevokeBySubjectAsync(_testData.Subject2)).ShouldBe(1);

        (await GetStatusesAsync(_tokenManager.FindAsync(_testData.Subject2, null, null, null))).ShouldBe(new[] { OpenIddictConstants.Statuses.Revoked });
    }

    [Fact]
    public async Task FindAsync_Should_Not_Cache_Uncommitted_Status_When_Unit_Of_Work_Is_Rolled_Back()
    {
        (await GetStatusesAsync(_tokenManager.FindAsync(_testData.Subject2, null, null, null))).ShouldBe(new[] { OpenIddictConstants.Statuses.Valid });

        // The test module disables transactions on IUnitOfWorkManager, so the transactional unit of work is started directly.
        using (ServiceProvider.GetRequiredService<UnitOfWorkManager>().Begin(new AbpUnitOfWorkOptions { IsTransactional = true }))
        {
            (await _tokenManager.RevokeBySubjectAsync(_testData.Subject2)).ShouldBe(1);
            (await GetStatusesAsync(_tokenManager.FindAsync(_testData.Subject2, null, null, null))).ShouldBe(new[] { OpenIddictConstants.Statuses.Revoked });
        }

        (await GetStatusAsync(_testData.Token2Id.ToString())).ShouldBe(OpenIddictConstants.Statuses.Valid);
    }

    private async Task<List<string>> GetStatusesAsync(IAsyncEnumerable<object> tokens)
    {
        var statuses = new List<string>();
        await foreach (var token in tokens)
        {
            statuses.Add(await _tokenManager.GetStatusAsync(token));
        }
        return statuses;
    }

    private async Task<string> GetStatusAsync(string id)
    {
        var token = await _tokenManager.FindByIdAsync(id);
        return await _tokenManager.GetStatusAsync(token);
    }
}
