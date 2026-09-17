using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using OpenIddict.Abstractions;
using Shouldly;
using Xunit;

namespace Volo.Abp.OpenIddict.Tokens;

public class AbpOpenIddictTokenStore_Tests : OpenIddictDomainTestBase
{
    private readonly IOpenIddictTokenStore<OpenIddictTokenModel> _tokenStore;
    private readonly AbpOpenIddictTestData _testData;

    public AbpOpenIddictTokenStore_Tests()
    {
        _tokenStore = ServiceProvider.GetRequiredService<IOpenIddictTokenStore<OpenIddictTokenModel>>();
        _testData = ServiceProvider.GetRequiredService<AbpOpenIddictTestData>();
    }

    [Fact]
    public async Task CountAsync()
    {
        var count = await _tokenStore.CountAsync(CancellationToken.None);
        count.ShouldBe(2);
    }

    [Fact]
    public async Task CreateAsync()
    {
        await _tokenStore.CreateAsync(new OpenIddictTokenModel
        {
            ApplicationId = _testData.App1Id,
            Payload = "TestPayload3",
            Subject = _testData.Subject3,
            Type = "TestType3",
            Status = OpenIddictConstants.Statuses.Inactive,

        }, CancellationToken.None);

        var tokens = await _tokenStore.FindBySubjectAsync(_testData.Subject3, CancellationToken.None).ToListAsync();

        tokens.Count.ShouldBe(1);
        var token = tokens.First();
        token.ApplicationId.ShouldBe(_testData.App1Id);
        token.Payload.ShouldBe("TestPayload3");
        token.Subject.ShouldBe(_testData.Subject3);
        token.Type.ShouldBe("TestType3");
        token.Status.ShouldBe(OpenIddictConstants.Statuses.Inactive);
    }

    [Fact]
    public async Task DeleteAsync()
    {
        var token = await _tokenStore.FindByIdAsync(_testData.Token1Id.ToString(), CancellationToken.None);
        token.ShouldNotBeNull();
        await _tokenStore.DeleteAsync(token, CancellationToken.None);

        token = await _tokenStore.FindByIdAsync(_testData.Token1Id.ToString(), CancellationToken.None);
        token.ShouldBeNull();
    }

    [Fact]
    public async Task FindAsync_Should_Return_Empty_If_Not_Found()
    {
        var tokens = await _tokenStore.FindAsync("non_existing_subject", _testData.App1Id.ToString(), "non_existing_status", "non_existing_type", CancellationToken.None).ToListAsync();

        tokens.Count.ShouldBe(0);
    }

    [Fact]
    public async Task FindAsync_Should_Return_Tokens_If_Found()
    {
        var tokens = await _tokenStore.FindAsync(_testData.Subject1, _testData.App1Id.ToString(),OpenIddictConstants.Statuses.Redeemed, "TestType1", CancellationToken.None).ToListAsync();

        tokens.Count.ShouldBe(1);
    }

    [Fact]
    public async Task FindByApplicationIdAsync_Should_Return_Empty_If_Not_Found()
    {
        var tokens = await _tokenStore.FindByApplicationIdAsync(Guid.NewGuid().ToString(), CancellationToken.None).ToListAsync();

        tokens.Count.ShouldBe(0);
    }

    [Fact]
    public async Task FindByApplicationIdAsync_Should_Return_Tokens_If_Found()
    {
        var tokens = await _tokenStore.FindByApplicationIdAsync(_testData.App1Id.ToString(), CancellationToken.None).ToListAsync();

        tokens.Count.ShouldBe(1);
    }

    [Fact]
    public async Task FindByIdAsync_Should_Return_Null_If_Not_Found()
    {
        var nonExistingId = Guid.NewGuid().ToString();
        var token = await _tokenStore.FindByIdAsync(nonExistingId, CancellationToken.None);
        token.ShouldBeNull();
    }

    [Fact]
    public async Task FindByIdAsync_Should_Return_Token_If_Found()
    {
        var token = await _tokenStore.FindByIdAsync(_testData.Token1Id.ToString(), CancellationToken.None);

        token.ShouldNotBeNull();
        token.ApplicationId.ShouldBe(_testData.App1Id);
        token.Payload.ShouldBe("TestPayload1");
        token.Subject.ShouldBe(_testData.Subject1);
        token.Type.ShouldBe("TestType1");
        token.Status.ShouldBe(OpenIddictConstants.Statuses.Redeemed);
        token.ExpirationDate.ShouldNotBeNull();
    }

    [Fact]
    public async Task FindByReferenceIdAsync_Should_Return_Null_If_Not_Found()
    {
        var token = await _tokenStore.FindByReferenceIdAsync(Guid.NewGuid().ToString(), CancellationToken.None);
        token.ShouldBeNull();
    }

    [Fact]
    public async Task FindByReferenceIdAsync_Should_Return_Token_If_Found()
    {
        var token = await _tokenStore.FindByIdAsync(_testData.Token1Id.ToString(), CancellationToken.None);
        token = await _tokenStore.FindByReferenceIdAsync(token.ReferenceId, CancellationToken.None);
        token.ShouldNotBeNull();
    }

    [Fact]
    public async Task UpdateAsync()
    {
        var token = await _tokenStore.FindByIdAsync(_testData.Token1Id.ToString(), CancellationToken.None);

        var now = DateTime.Now;
        token.ApplicationId = _testData.App2Id;
        token.Payload = "New payload";
        token.Status = OpenIddictConstants.Statuses.Revoked;
        token.Subject = "New subject";
        token.Type = "New type";
        token.ExpirationDate = now;

        await _tokenStore.UpdateAsync(token, CancellationToken.None);
        token = await _tokenStore.FindByIdAsync(_testData.Token1Id.ToString(), CancellationToken.None);

        token.ApplicationId.ShouldBe(_testData.App2Id);
        token.Payload.ShouldBe("New payload");
        token.Subject.ShouldBe("New subject");
        token.Type.ShouldBe("New type");
        token.Status.ShouldBe(OpenIddictConstants.Statuses.Revoked);
        token.ExpirationDate.ShouldBe(now);
    }
}
