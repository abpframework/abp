using System;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Shouldly;
using Volo.Abp.Uow;
using Xunit;

namespace Volo.Abp.Identity.AspNetCore;

/// <summary>
/// Pins the payload of <see cref="AbpSingleActiveTokenProvider"/> to ASP.NET Core's
/// <see cref="DataProtectorTokenProvider{TUser}"/>, which it re-implements rather than derives from:
/// a token produced by one has to be accepted by the other under the same provider name.
/// </summary>
public class AbpSingleActiveTokenProvider_Compatibility_Tests : AbpIdentityAspNetCoreTestBase
{
    private const string Purpose = "ResetPassword";

    private readonly IIdentityUserRepository _userRepository;
    private readonly IdentityUserManager _userManager;
    private readonly IdentityTestData _testData;
    private readonly IUnitOfWorkManager _unitOfWorkManager;

    public AbpSingleActiveTokenProvider_Compatibility_Tests()
    {
        _userRepository = GetRequiredService<IIdentityUserRepository>();
        _userManager = GetRequiredService<IdentityUserManager>();
        _testData = GetRequiredService<IdentityTestData>();
        _unitOfWorkManager = GetRequiredService<IUnitOfWorkManager>();
    }

    [Fact]
    public async Task AspNetCore_Provider_Should_Validate_A_Token_Generated_By_Abp()
    {
        using var uow = _unitOfWorkManager.Begin();

        var user = await _userRepository.GetAsync(_testData.UserJohnId);
        var token = await _userManager.GeneratePasswordResetTokenAsync(user);

        user = await _userRepository.GetAsync(_testData.UserJohnId);
        (await CreateAspNetCoreProvider().ValidateAsync(Purpose, token, _userManager, user)).ShouldBeTrue();

        await uow.CompleteAsync();
    }

    [Fact]
    public async Task Abp_Provider_Should_Validate_A_Token_Generated_By_AspNetCore()
    {
        using var uow = _unitOfWorkManager.Begin();

        var user = await _userRepository.GetAsync(_testData.UserJohnId);
        var token = await CreateAspNetCoreProvider().GenerateAsync(Purpose, _userManager, user);

        // The ABP provider additionally requires the stored hash the generating side would have written.
        (await _userManager.SetAuthenticationTokenAsync(
            user,
            AbpSingleActiveTokenProvider.InternalLoginProvider,
            AbpPasswordResetTokenProvider.ProviderName + ":" + Purpose,
            Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(token))))).Succeeded.ShouldBeTrue();

        user = await _userRepository.GetAsync(_testData.UserJohnId);
        (await _userManager.VerifyUserTokenAsync(
            user,
            AbpPasswordResetTokenProvider.ProviderName,
            Purpose,
            token)).ShouldBeTrue();

        await uow.CompleteAsync();
    }

    private DataProtectorTokenProvider<IdentityUser> CreateAspNetCoreProvider()
    {
        var abpOptions = GetRequiredService<IOptions<AbpPasswordResetTokenProviderOptions>>().Value;

        return new DataProtectorTokenProvider<IdentityUser>(
            GetRequiredService<IDataProtectionProvider>(),
            Microsoft.Extensions.Options.Options.Create(new DataProtectionTokenProviderOptions
            {
                Name = abpOptions.Name,
                TokenLifespan = abpOptions.TokenLifespan
            }),
            GetRequiredService<ILogger<DataProtectorTokenProvider<IdentityUser>>>());
    }
}
