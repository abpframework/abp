using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Shouldly;
using Volo.Abp.Uow;
using Xunit;

namespace Volo.Abp.Identity;

public class IdentityUserStore_Tests : AbpIdentityDomainTestBase
{
    private readonly IdentityUserStore _identityUserStore;
    private readonly ILookupNormalizer _lookupNormalizer;
    private readonly IIdentityUserRepository _userRepository;
    private readonly IdentityTestData _testData;
    private readonly IUnitOfWorkManager _unitOfWorkManager;

    public IdentityUserStore_Tests()
    {
        _identityUserStore = GetRequiredService<IdentityUserStore>();
        _lookupNormalizer = GetRequiredService<ILookupNormalizer>();
        _userRepository = GetRequiredService<IIdentityUserRepository>();
        _testData = GetRequiredService<IdentityTestData>();
        _unitOfWorkManager = GetRequiredService<IUnitOfWorkManager>();
    }

    [Fact]
    public async Task GetUserIdAsync()
    {
        var user = await _userRepository.FindByNormalizedUserNameAsync(_lookupNormalizer.NormalizeName("john.nash"));
        user.ShouldNotBeNull();

        (await _identityUserStore.GetUserIdAsync(user)).ShouldBe(user.Id.ToString());
    }


    [Fact]
    public async Task GetUserNameAsync()
    {
        var user = await _userRepository.FindByNormalizedUserNameAsync(_lookupNormalizer.NormalizeName("john.nash"));
        user.ShouldNotBeNull();

        (await _identityUserStore.GetUserNameAsync(user)).ShouldBe(user.UserName);
    }

    [Fact]
    public async Task SetUserNameAsync()
    {
        var user = await _userRepository.FindByNormalizedUserNameAsync(_lookupNormalizer.NormalizeName("john.nash"));
        user.ShouldNotBeNull();

        await _identityUserStore.SetUserNameAsync(user, "bob.lee");
        user.UserName.ShouldBe("bob.lee");
        //user.NormalizedUserName.ShouldBe(_lookupNormalizer.Normalize("bob.lee"));
    }


    [Fact]
    public async Task GetNormalizedUserNameAsync()
    {
        var user = await _userRepository.FindByNormalizedUserNameAsync(_lookupNormalizer.NormalizeName("john.nash"));
        user.ShouldNotBeNull();

        (await _identityUserStore.GetNormalizedUserNameAsync(user)).ShouldBe(user.NormalizedUserName);
    }

    [Fact]
    public async Task SetNormalizedUserNameAsync()
    {
        var user = await _userRepository.FindByNormalizedUserNameAsync(_lookupNormalizer.NormalizeName("john.nash"));
        user.ShouldNotBeNull();

        await _identityUserStore.SetNormalizedUserNameAsync(user, _lookupNormalizer.NormalizeName("bob.lee"));

        user.NormalizedUserName.ShouldBe(_lookupNormalizer.NormalizeName("bob.lee"));
    }

    [Fact]
    public async Task CreateAsync()
    {
        var userId = Guid.NewGuid();
        var user = new IdentityUser(userId, "bob.lee", "bob.lee@abp.io");

        await _identityUserStore.CreateAsync(user);

        var bobLee = await _userRepository.FindByNormalizedUserNameAsync(_lookupNormalizer.NormalizeName("bob.lee"));
        bobLee.ShouldNotBeNull();
        bobLee.UserName.ShouldBe("bob.lee");
        bobLee.Email.ShouldBe("bob.lee@abp.io");
    }

    [Fact]
    public async Task UpdateAsync()
    {
        var user = await _userRepository.FindByNormalizedUserNameAsync(_lookupNormalizer.NormalizeName("john.nash"));
        user.ShouldNotBeNull();

        user.Name = "lee";
        (await _identityUserStore.UpdateAsync(user)).Succeeded.ShouldBeTrue();

        (await _userRepository.FindByNormalizedUserNameAsync(_lookupNormalizer.NormalizeName("john.nash"))).Name
            .ShouldBe("lee");
    }

    [Fact]
    public async Task Update_Concurrency()
    {
        var user = await _userRepository.FindByNormalizedUserNameAsync(_lookupNormalizer.NormalizeName("john.nash"));
        user.ShouldNotBeNull();

        (await _identityUserStore.UpdateAsync(user)).Succeeded.ShouldBeTrue();

        user.ConcurrencyStamp = Guid.NewGuid().ToString();
        var identityResult = await _identityUserStore.UpdateAsync(user);

        identityResult.Succeeded.ShouldBeFalse();
        identityResult.Errors.ShouldContain(x =>
            x.Code == nameof(IdentityErrorDescriber.ConcurrencyFailure));

        // && x.Description == Resources.PasswordMismatch);
    }


    [Fact]
    public async Task DeleteAsync()
    {
        var user = await _userRepository.FindByNormalizedUserNameAsync(_lookupNormalizer.NormalizeName("john.nash"));
        user.ShouldNotBeNull();

        (await _identityUserStore.DeleteAsync(user)).Succeeded.ShouldBeTrue();

        (await _userRepository.FindByNormalizedUserNameAsync(_lookupNormalizer.NormalizeName("john.nash"))).ShouldBeNull();
    }


    [Fact]
    public async Task FindByIdAsync()
    {
        var user = await _identityUserStore.FindByIdAsync(_testData.UserJohnId.ToString());

        user.ShouldNotBeNull();
        user.UserName.ShouldBe("john.nash");
    }


    [Fact]
    public async Task FindByNameAsync()
    {
        var user = await _identityUserStore.FindByNameAsync(_lookupNormalizer.NormalizeName("john.nash"));

        user.ShouldNotBeNull();
        user.UserName.ShouldBe("john.nash");
    }


    [Fact]
    public async Task SetPasswordHashAsync()
    {
        var user = await _userRepository.FindByNormalizedUserNameAsync(_lookupNormalizer.NormalizeName("john.nash"));
        user.ShouldNotBeNull();
        user.PasswordHash.ShouldBeNull();

        await _identityUserStore.SetPasswordHashAsync(user, "P@ssw0rd");
        user.PasswordHash.ShouldBe("P@ssw0rd");
    }


    [Fact]
    public async Task GetPasswordHashAsync()
    {
        var user = await _userRepository.FindByNormalizedUserNameAsync(_lookupNormalizer.NormalizeName("john.nash"));
        user.ShouldNotBeNull();
        await _identityUserStore.SetPasswordHashAsync(user, "P@ssw0rd");

        (await _identityUserStore.GetPasswordHashAsync(user)).ShouldBe("P@ssw0rd");
    }


    [Fact]
    public async Task HasPasswordAsync()
    {
        var user = await _userRepository.FindByNormalizedUserNameAsync(_lookupNormalizer.NormalizeName("john.nash"));
        user.ShouldNotBeNull();
        (await _identityUserStore.HasPasswordAsync(user)).ShouldBeFalse();

        await _identityUserStore.SetPasswordHashAsync(user, "P@ssw0rd");
        (await _identityUserStore.HasPasswordAsync(user)).ShouldBeTrue();
    }

    [Fact]
    public async Task AddToRoleAsync()
    {
        using (var uow = _unitOfWorkManager.Begin())
        {
            var user = await _userRepository.FindByNormalizedUserNameAsync(_lookupNormalizer.NormalizeName("david"));
            user.ShouldNotBeNull();
            user.Roles.ShouldBeEmpty();

            await _identityUserStore.AddToRoleAsync(user, _lookupNormalizer.NormalizeName("moderator"));

            user.Roles.ShouldContain(x => x.RoleId == _testData.RoleModeratorId);

            await uow.CompleteAsync();
        }
    }

    [Fact]
    public async Task RemoveFromRoleAsync()
    {
        using (var uow = _unitOfWorkManager.Begin())
        {
            var user = await _userRepository.FindByNormalizedUserNameAsync(_lookupNormalizer.NormalizeName("john.nash"));
            user.ShouldNotBeNull();
            user.Roles.ShouldContain(x => x.RoleId == _testData.RoleModeratorId);

            await _identityUserStore.RemoveFromRoleAsync(user, _lookupNormalizer.NormalizeName("moderator"));

            user.Roles.ShouldNotContain(x => x.RoleId == _testData.RoleModeratorId);

            await uow.CompleteAsync();
        }
    }


    [Fact]
    public async Task GetRolesAsync()
    {
        var user = await _userRepository.FindByNormalizedUserNameAsync(_lookupNormalizer.NormalizeName("john.nash"));
        user.ShouldNotBeNull();

        var roleNames = await _identityUserStore.GetRolesAsync(user);
        roleNames.ShouldNotBeEmpty();
        roleNames.ShouldContain(x => x == "moderator");
        roleNames.ShouldContain(x => x == "supporter");
    }

    [Fact]
    public async Task IsInRoleAsync()
    {
        using (var uow = _unitOfWorkManager.Begin())
        {
            var user = await _userRepository.FindByNormalizedUserNameAsync(_lookupNormalizer.NormalizeName("john.nash"));
            user.ShouldNotBeNull();

            (await _identityUserStore.IsInRoleAsync(user, _lookupNormalizer.NormalizeName("moderator"))).ShouldBeTrue();
            (await _identityUserStore.IsInRoleAsync(user, _lookupNormalizer.NormalizeName("moderatorNotExist"))).ShouldBeFalse();

            await uow.CompleteAsync();
        }
    }

    [Fact]
    public async Task GetClaimsAsync()
    {
        using (var uow = _unitOfWorkManager.Begin())
        {
            var user = await _userRepository.FindByNormalizedUserNameAsync(_lookupNormalizer.NormalizeName("john.nash"));
            user.ShouldNotBeNull();

            var claims = await _identityUserStore.GetClaimsAsync(user);
            claims.ShouldNotBeEmpty();
            claims.ShouldContain(x => x.Type == "TestClaimType" && x.Value == "42");

            await uow.CompleteAsync();
        }
    }

    [Fact]
    public async Task AddClaimsAsync()
    {
        using (var uow = _unitOfWorkManager.Begin())
        {
            var user = await _userRepository.FindByNormalizedUserNameAsync(_lookupNormalizer.NormalizeName("john.nash"));
            user.ShouldNotBeNull();
            user.Claims.ShouldNotContain(x => x.ClaimType == "MyClaimType" && x.ClaimValue == "MyClaimValue");

            await _identityUserStore.AddClaimsAsync(user, new List<Claim>()
                {
                    new Claim("MyClaimType", "MyClaimValue")
                });

            user.Claims.ShouldContain(x => x.ClaimType == "MyClaimType" && x.ClaimValue == "MyClaimValue");

            await uow.CompleteAsync();
        }
    }

    [Fact]
    public async Task ReplaceClaimAsync()
    {
        using (var uow = _unitOfWorkManager.Begin())
        {
            var user = await _userRepository.FindByNormalizedUserNameAsync(_lookupNormalizer.NormalizeName("john.nash"));
            user.ShouldNotBeNull();

            await _identityUserStore.ReplaceClaimAsync(user, new Claim("TestClaimType", "42"), new Claim("MyClaimType", "MyClaimValue"));

            user.Claims.ShouldNotContain(x => x.ClaimType == "TestClaimType" && x.ClaimValue == "42");
            user.Claims.ShouldContain(x => x.ClaimType == "MyClaimType" && x.ClaimValue == "MyClaimValue");

            await uow.CompleteAsync();
        }
    }

    [Fact]
    public async Task RemoveClaimsAsync()
    {
        using (var uow = _unitOfWorkManager.Begin())
        {
            var user = await _userRepository.FindByNormalizedUserNameAsync(_lookupNormalizer.NormalizeName("john.nash"));
            user.ShouldNotBeNull();

            await _identityUserStore.RemoveClaimsAsync(user, new List<Claim>()
                {
                    new Claim("TestClaimType", "42")
                });

            user.Claims.ShouldNotContain(x => x.ClaimType == "TestClaimType" && x.ClaimValue == "42");

            await uow.CompleteAsync();
        }
    }

    [Fact]
    public async Task AddLoginAsync()
    {
        using (var uow = _unitOfWorkManager.Begin())
        {
            var user = await _userRepository.FindByNormalizedUserNameAsync(_lookupNormalizer.NormalizeName("john.nash"));
            user.ShouldNotBeNull();
            user.Logins.ShouldNotContain(x => x.LoginProvider == "facebook" && x.ProviderKey == "john");

            await _identityUserStore.AddLoginAsync(user, new UserLoginInfo("facebook", "john", "John Nash"));

            user.Logins.ShouldContain(x => x.LoginProvider == "facebook" && x.ProviderKey == "john");

            await uow.CompleteAsync();
        }
    }

    [Fact]
    public async Task RemoveLoginAsync()
    {
        using (var uow = _unitOfWorkManager.Begin())
        {
            var user = await _userRepository.FindByNormalizedUserNameAsync(_lookupNormalizer.NormalizeName("john.nash"));
            user.ShouldNotBeNull();
            user.Logins.ShouldContain(x => x.LoginProvider == "github" && x.ProviderKey == "john");

            await _identityUserStore.RemoveLoginAsync(user, "github", "john");

            user.Logins.ShouldNotContain(x => x.LoginProvider == "github" && x.ProviderKey == "john");

            await uow.CompleteAsync();
        }
    }


    [Fact]
    public async Task GetLoginsAsync()
    {
        using (var uow = _unitOfWorkManager.Begin())
        {
            var user = await _userRepository.FindByNormalizedUserNameAsync(_lookupNormalizer.NormalizeName("john.nash"));
            user.ShouldNotBeNull();

            var logins = await _identityUserStore.GetLoginsAsync(user);

            logins.ShouldNotBeNull();
            logins.ShouldContain(x => x.LoginProvider == "github" && x.ProviderKey == "john");
            logins.ShouldContain(x => x.LoginProvider == "twitter" && x.ProviderKey == "johnx");

            await uow.CompleteAsync();
        }
    }


    [Fact]
    public async Task FindByLoginAsync()
    {
        var user = await _identityUserStore.FindByLoginAsync("github", "john");
        user.ShouldNotBeNull();
        user.UserName.ShouldBe("john.nash");
    }


    [Fact]
    public async Task GetEmailConfirmedAsync()
    {
        var user = await _userRepository.FindByNormalizedUserNameAsync(_lookupNormalizer.NormalizeName("john.nash"));
        user.ShouldNotBeNull();

        (await _identityUserStore.GetEmailConfirmedAsync(user)).ShouldBe(user.EmailConfirmed);
    }


    [Fact]
    public async Task SetEmailConfirmedAsync()
    {
        var user = await _userRepository.FindByNormalizedUserNameAsync(_lookupNormalizer.NormalizeName("john.nash"));
        user.ShouldNotBeNull();
        var emailConfirmed = user.EmailConfirmed;

        await _identityUserStore.SetEmailConfirmedAsync(user, !emailConfirmed);

        user.EmailConfirmed.ShouldBe(!emailConfirmed);
    }

    [Fact]
    public async Task SetEmailAsync()
    {
        var user = await _userRepository.FindByNormalizedUserNameAsync(_lookupNormalizer.NormalizeName("john.nash"));
        user.ShouldNotBeNull();

        await _identityUserStore.SetEmailAsync(user, "john.nash.kobe@abp.io");

        user.Email.ShouldBe("john.nash.kobe@abp.io");
        //user.NormalizedEmail.ShouldBe(_lookupNormalizer.Normalize("john.nash.kobe@abp.io"));
    }

    [Fact]
    public async Task GetEmailAsync()
    {
        var user = await _userRepository.FindByNormalizedUserNameAsync(_lookupNormalizer.NormalizeName("john.nash"));
        user.ShouldNotBeNull();

        (await _identityUserStore.GetEmailAsync(user)).ShouldBe(user.Email);
    }

    [Fact]
    public async Task GetNormalizedEmailAsync()
    {
        var user = await _userRepository.FindByNormalizedUserNameAsync(_lookupNormalizer.NormalizeName("john.nash"));
        user.ShouldNotBeNull();

        (await _identityUserStore.GetNormalizedEmailAsync(user)).ShouldBe(user.NormalizedEmail);
    }

    [Fact]
    public async Task SetNormalizedEmailAsync()
    {
        var user = await _userRepository.FindByNormalizedUserNameAsync(_lookupNormalizer.NormalizeName("john.nash"));
        user.ShouldNotBeNull();

        await _identityUserStore.SetNormalizedEmailAsync(user, _lookupNormalizer.NormalizeEmail("john.nash.kobe@abp.io"));

        user.NormalizedEmail.ShouldBe(_lookupNormalizer.NormalizeEmail("john.nash.kobe@abp.io"));
    }

    [Fact]
    public async Task FindByEmailAsync()
    {
        var user = await _identityUserStore.FindByEmailAsync(_lookupNormalizer.NormalizeEmail("john.nash@abp.io"));

        user.ShouldNotBeNull();
        user.Email.ShouldBe("john.nash@abp.io");
    }

    [Fact]
    public async Task GetLockoutEndDateAsync()
    {
        var user = await _userRepository.FindByNormalizedUserNameAsync(_lookupNormalizer.NormalizeName("john.nash"));
        user.ShouldNotBeNull();

        (await _identityUserStore.GetLockoutEndDateAsync(user)).ShouldBe(user.LockoutEnd);
    }

    [Fact]
    public async Task SetLockoutEndDateAsync()
    {
        var user = await _userRepository.FindByNormalizedUserNameAsync(_lookupNormalizer.NormalizeName("john.nash"));
        user.ShouldNotBeNull();

        await _identityUserStore.SetLockoutEndDateAsync(user, DateTimeOffset.Parse("01/01/2019"));

        user.LockoutEnd.ShouldBe(DateTimeOffset.Parse("01/01/2019"));
    }


    [Fact]
    public async Task IncrementAccessFailedCountAsync()
    {
        var user = await _userRepository.FindByNormalizedUserNameAsync(_lookupNormalizer.NormalizeName("john.nash"));
        user.ShouldNotBeNull();
        var count = user.AccessFailedCount;

        await _identityUserStore.IncrementAccessFailedCountAsync(user);

        user.AccessFailedCount.ShouldBe(count + 1);
    }

    [Fact]
    public async Task ResetAccessFailedCountAsync()
    {
        var user = await _userRepository.FindByNormalizedUserNameAsync(_lookupNormalizer.NormalizeName("john.nash"));
        user.ShouldNotBeNull();

        await _identityUserStore.ResetAccessFailedCountAsync(user);

        user.AccessFailedCount.ShouldBe(0);
    }

    [Fact]
    public async Task GetAccessFailedCountAsync()
    {
        var user = await _userRepository.FindByNormalizedUserNameAsync(_lookupNormalizer.NormalizeName("john.nash"));
        user.ShouldNotBeNull();

        (await _identityUserStore.GetAccessFailedCountAsync(user)).ShouldBe(user.AccessFailedCount);
    }

    [Fact]
    public async Task GetLockoutEnabledAsync()
    {
        var user = await _userRepository.FindByNormalizedUserNameAsync(_lookupNormalizer.NormalizeName("john.nash"));
        user.ShouldNotBeNull();

        (await _identityUserStore.GetLockoutEnabledAsync(user)).ShouldBe(user.LockoutEnabled);
    }

    [Fact]
    public async Task SetLockoutEnabledAsync()
    {
        var user = await _userRepository.FindByNormalizedUserNameAsync(_lookupNormalizer.NormalizeName("john.nash"));
        user.ShouldNotBeNull();
        var lockoutEnabled = user.LockoutEnabled;

        await _identityUserStore.SetLockoutEnabledAsync(user, !lockoutEnabled);

        user.LockoutEnabled.ShouldBe(!lockoutEnabled);
    }


    [Fact]
    public async Task SetPhoneNumberAsync()
    {
        var user = await _userRepository.FindByNormalizedUserNameAsync(_lookupNormalizer.NormalizeName("john.nash"));
        user.ShouldNotBeNull();

        await _identityUserStore.SetPhoneNumberAsync(user, "13800138000");

        user.PhoneNumber.ShouldBe("13800138000");
    }


    [Fact]
    public async Task GetPhoneNumberAsync()
    {
        var user = await _userRepository.FindByNormalizedUserNameAsync(_lookupNormalizer.NormalizeName("john.nash"));
        user.ShouldNotBeNull();

        (await _identityUserStore.GetPhoneNumberAsync(user)).ShouldBe(user.PhoneNumber);
    }

    [Fact]
    public async Task GetPhoneNumberConfirmedAsync()
    {
        var user = await _userRepository.FindByNormalizedUserNameAsync(_lookupNormalizer.NormalizeName("john.nash"));
        user.ShouldNotBeNull();

        (await _identityUserStore.GetPhoneNumberConfirmedAsync(user)).ShouldBe(user.PhoneNumberConfirmed);
    }


    [Fact]
    public async Task SetPhoneNumberConfirmedAsync()
    {
        var user = await _userRepository.FindByNormalizedUserNameAsync(_lookupNormalizer.NormalizeName("john.nash"));
        user.ShouldNotBeNull();
        var phoneNumberConfirmed = user.PhoneNumberConfirmed;

        await _identityUserStore.SetPhoneNumberConfirmedAsync(user, !phoneNumberConfirmed);

        user.PhoneNumberConfirmed.ShouldBe(!phoneNumberConfirmed);
    }


    [Fact]
    public async Task SetSecurityStampAsync()
    {
        var user = await _userRepository.FindByNormalizedUserNameAsync(_lookupNormalizer.NormalizeName("john.nash"));
        user.ShouldNotBeNull();
        var securityStamp = Guid.NewGuid().ToString();

        await _identityUserStore.SetSecurityStampAsync(user, securityStamp);

        user.SecurityStamp.ShouldBe(securityStamp);
    }

    [Fact]
    public async Task GetSecurityStampAsync()
    {
        var user = await _userRepository.FindByNormalizedUserNameAsync(_lookupNormalizer.NormalizeName("john.nash"));
        user.ShouldNotBeNull();

        (await _identityUserStore.GetSecurityStampAsync(user)).ShouldBe(user.SecurityStamp);
    }


    [Fact]
    public async Task SetTwoFactorEnabledAsync()
    {
        var user = await _userRepository.FindByNormalizedUserNameAsync(_lookupNormalizer.NormalizeName("john.nash"));
        user.ShouldNotBeNull();
        var twoFactorEnabled = user.TwoFactorEnabled;

        await _identityUserStore.SetTwoFactorEnabledAsync(user, !twoFactorEnabled);

        user.TwoFactorEnabled.ShouldBe(!twoFactorEnabled);
    }


    [Fact]
    public async Task GetTwoFactorEnabledAsync()
    {
        var user = await _userRepository.FindByNormalizedUserNameAsync(_lookupNormalizer.NormalizeName("john.nash"));
        user.ShouldNotBeNull();

        (await _identityUserStore.GetTwoFactorEnabledAsync(user)).ShouldBe(user.TwoFactorEnabled);
    }

    [Fact]
    public async Task GetUsersForClaimAsync()
    {
        var user = await _identityUserStore.GetUsersForClaimAsync(new Claim("TestClaimType", "42"));
        user.ShouldNotBeNull();
        user.ShouldNotBeEmpty();
        user.ShouldContain(x => x.UserName == "john.nash");
    }

    [Fact]
    public async Task GetUsersInRoleAsync()
    {
        var user = await _identityUserStore.GetUsersInRoleAsync(_lookupNormalizer.NormalizeName("moderator"));
        user.ShouldNotBeNull();
        user.ShouldContain(x => x.UserName == "john.nash");
    }

    [Fact]
    public async Task SetTokenAsync()
    {
        using (var uow = _unitOfWorkManager.Begin())
        {
            var user = await _userRepository.FindByNormalizedUserNameAsync(_lookupNormalizer.NormalizeName("john.nash"));
            user.ShouldNotBeNull();

            await _identityUserStore.SetTokenAsync(user, "test-provider", "test-name", "123123");

            user.Tokens.ShouldNotBeEmpty();
            user.Tokens.ShouldContain(x => x.LoginProvider == "test-provider" && x.Name == "test-name" && x.Value == "123123");

            await uow.CompleteAsync();
        }
    }

    [Fact]
    public async Task RemoveTokenAsync()
    {
        using (var uow = _unitOfWorkManager.Begin())
        {
            var user = await _userRepository.FindByNormalizedUserNameAsync(_lookupNormalizer.NormalizeName("john.nash"));
            user.ShouldNotBeNull();

            await _identityUserStore.RemoveTokenAsync(user, "test-provider", "test-name");

            user.Tokens.ShouldNotContain(x => x.LoginProvider == "test-provider" && x.Name == "test-name" && x.Value == "123123");

            await uow.CompleteAsync();
        }
    }

    [Fact]
    public async Task GetTokenAsync()
    {
        using (var uow = _unitOfWorkManager.Begin())
        {
            var user = await _userRepository.FindByNormalizedUserNameAsync(_lookupNormalizer.NormalizeName("john.nash"));
            user.ShouldNotBeNull();

            (await _identityUserStore.GetTokenAsync(user, "test-provider", "test-name")).ShouldBe("test-value");

            await uow.CompleteAsync();
        }
    }

    [Fact]
    public async Task SetAuthenticatorKeyAsync()
    {
        using (var uow = _unitOfWorkManager.Begin())
        {
            var user = await _userRepository.FindByNormalizedUserNameAsync(_lookupNormalizer.NormalizeName("john.nash"));
            user.ShouldNotBeNull();

            await _identityUserStore.SetAuthenticatorKeyAsync(user, "testKey");

            user.Tokens.ShouldContain(x => x.LoginProvider == "[AspNetUserStore]" && x.Name == "AuthenticatorKey" && x.Value == "testKey");

            await uow.CompleteAsync();
        }
    }

    [Fact]
    public async Task GetAuthenticatorKeyAsync()
    {
        using (var uow = _unitOfWorkManager.Begin())
        {
            var user = await _userRepository.FindByNormalizedUserNameAsync(_lookupNormalizer.NormalizeName("john.nash"));
            user.ShouldNotBeNull();
            await _identityUserStore.SetAuthenticatorKeyAsync(user, "testKey");

            (await _identityUserStore.GetAuthenticatorKeyAsync(user)).ShouldBe("testKey");

            await uow.CompleteAsync();
        }

    }

    [Fact]
    public async Task CountCodesAsync()
    {
        using (var uow = _unitOfWorkManager.Begin())
        {
            var user = await _userRepository.FindByNormalizedUserNameAsync(_lookupNormalizer.NormalizeName("john.nash"));
            user.ShouldNotBeNull();
            await _identityUserStore.SetTokenAsync(user, "[AspNetUserStore]", "RecoveryCodes", "testKey;testKey2");

            (await _identityUserStore.CountCodesAsync(user)).ShouldBe(2);

            await uow.CompleteAsync();
        }
    }

    [Fact]
    public async Task ReplaceCodesAsync()
    {
        using (var uow = _unitOfWorkManager.Begin())
        {
            var user = await _userRepository.FindByNormalizedUserNameAsync(_lookupNormalizer.NormalizeName("john.nash"));
            user.ShouldNotBeNull();

            await _identityUserStore.ReplaceCodesAsync(user, new List<string>()
                {
                    "testKey",
                    "testKey2"
                });

            user.Tokens.ShouldContain(x => x.LoginProvider == "[AspNetUserStore]" && x.Name == "RecoveryCodes" && x.Value == "testKey;testKey2");

            await uow.CompleteAsync();
        }
    }

    [Fact]
    public async Task RedeemCodeAsync()
    {
        using (var uow = _unitOfWorkManager.Begin())
        {
            var user = await _userRepository.FindByNormalizedUserNameAsync(_lookupNormalizer.NormalizeName("john.nash"));
            user.ShouldNotBeNull();
            await _identityUserStore.SetTokenAsync(user, "[AspNetUserStore]", "RecoveryCodes", "testKey;testKey2");

            (await _identityUserStore.RedeemCodeAsync(user, "testKey")).ShouldBeTrue();

            await uow.CompleteAsync();
        }
    }
}
