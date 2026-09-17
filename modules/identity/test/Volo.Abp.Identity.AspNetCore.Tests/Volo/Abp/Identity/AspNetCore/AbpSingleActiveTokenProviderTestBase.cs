using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Shouldly;
using Volo.Abp.Uow;
using Xunit;

namespace Volo.Abp.Identity.AspNetCore;

/// <summary>
/// Abstract base class that exercises the common behaviour of every
/// <see cref="AbpSingleActiveTokenProvider"/> subclass.
/// Concrete subclasses inject their provider-specific generate/verify helpers
/// so the same test suite runs against each provider.
/// </summary>
public abstract class AbpSingleActiveTokenProviderTestBase : AbpIdentityAspNetCoreTestBase
{
    protected IIdentityUserRepository UserRepository { get; }
    protected IdentityUserManager UserManager { get; }
    protected IdentityTestData TestData { get; }
    protected IUnitOfWorkManager UnitOfWorkManager { get; }

    protected AbpSingleActiveTokenProviderTestBase()
    {
        UserRepository = GetRequiredService<IIdentityUserRepository>();
        UserManager = GetRequiredService<IdentityUserManager>();
        TestData = GetRequiredService<IdentityTestData>();
        UnitOfWorkManager = GetRequiredService<IUnitOfWorkManager>();
    }

    /// <summary>Generates a token for <paramref name="user"/> via the provider under test.</summary>
    protected abstract Task<string> GenerateTokenAsync(IdentityUser user);

    /// <summary>Verifies <paramref name="token"/> for <paramref name="user"/> via the provider under test.</summary>
    protected abstract Task<bool> VerifyTokenAsync(IdentityUser user, string token);

    /// <summary>Returns the provider name used to look up the stored hash.</summary>
    protected abstract string GetProviderName();

    /// <summary>Returns the token purpose used as the hash key prefix.</summary>
    protected abstract string GetPurpose();

    private string GetTokenHashName() => GetProviderName() + ":" + GetPurpose();

    [Fact]
    public async Task Generate_And_Verify_Token_Should_Succeed()
    {
        using (var uow = UnitOfWorkManager.Begin())
        {
            var user = await UserRepository.GetAsync(TestData.UserJohnId);

            var token = await GenerateTokenAsync(user);
            token.ShouldNotBeNullOrEmpty();

            user = await UserRepository.GetAsync(TestData.UserJohnId);
            (await VerifyTokenAsync(user, token)).ShouldBeTrue();

            await uow.CompleteAsync();
        }
    }

    [Fact]
    public async Task Invalid_Token_Should_Fail_Verification()
    {
        using (var uow = UnitOfWorkManager.Begin())
        {
            var user = await UserRepository.GetAsync(TestData.UserJohnId);
            await GenerateTokenAsync(user);

            user = await UserRepository.GetAsync(TestData.UserJohnId);
            (await VerifyTokenAsync(user, "invalid-token-value")).ShouldBeFalse();

            await uow.CompleteAsync();
        }
    }

    [Fact]
    public async Task Second_Token_Should_Invalidate_First_Token()
    {
        using (var uow = UnitOfWorkManager.Begin())
        {
            var user = await UserRepository.GetAsync(TestData.UserJohnId);
            var firstToken = await GenerateTokenAsync(user);

            user = await UserRepository.GetAsync(TestData.UserJohnId);
            var secondToken = await GenerateTokenAsync(user);

            user = await UserRepository.GetAsync(TestData.UserJohnId);

            (await VerifyTokenAsync(user, firstToken)).ShouldBeFalse();
            (await VerifyTokenAsync(user, secondToken)).ShouldBeTrue();

            await uow.CompleteAsync();
        }
    }

    [Fact]
    public async Task Corrupted_Hash_Should_Return_False_Instead_Of_Throwing()
    {
        using (var uow = UnitOfWorkManager.Begin())
        {
            var user = await UserRepository.GetAsync(TestData.UserJohnId);
            var token = await GenerateTokenAsync(user);

            // Overwrite with a non-hex string to simulate data corruption.
            user = await UserRepository.GetAsync(TestData.UserJohnId);
            await UserManager.SetAuthenticationTokenAsync(user, AbpSingleActiveTokenProvider.InternalLoginProvider, GetTokenHashName(), "not-valid-hex!!!");

            user = await UserRepository.GetAsync(TestData.UserJohnId);

            // ValidateAsync must catch FormatException internally and return false.
            (await VerifyTokenAsync(user, token)).ShouldBeFalse();

            await uow.CompleteAsync();
        }
    }

    [Fact]
    public async Task Token_Hash_Should_Persist_Across_UnitOfWork_Boundaries()
    {
        string token;

        // UoW 1: generate; UpdateAsync inside GenerateAsync must write the hash to the DB.
        using (var uow = UnitOfWorkManager.Begin(requiresNew: true))
        {
            var user = await UserRepository.GetAsync(TestData.UserJohnId);
            token = await GenerateTokenAsync(user);
            await uow.CompleteAsync();
        }

        // UoW 2: validate with a fresh DbContext to confirm the hash was persisted.
        using (var uow = UnitOfWorkManager.Begin(requiresNew: true))
        {
            var user = await UserRepository.GetAsync(TestData.UserJohnId);
            (await VerifyTokenAsync(user, token)).ShouldBeTrue();
            await uow.CompleteAsync();
        }
    }
}
