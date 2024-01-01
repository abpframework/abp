using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using NSubstitute;
using Shouldly;
using Volo.Abp.EventBus.Distributed;
using Volo.Abp.Identity.Settings;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Uow;
using Xunit;

namespace Volo.Abp.Identity;

public class IdentityUserManager_Tests : AbpIdentityDomainTestBase
{
    private readonly IdentityUserManager _identityUserManager;
    private readonly IIdentityUserRepository _identityUserRepository;
    private readonly IIdentityRoleRepository _identityRoleRepository;
    private readonly IOrganizationUnitRepository _organizationUnitRepository;
    private readonly ILookupNormalizer _lookupNormalizer;
    private readonly IUnitOfWorkManager _unitOfWorkManager;
    private readonly IdentityTestData _testData;
    private readonly ICurrentTenant _currentTenant;
    private readonly IOptions<IdentityOptions> _identityOptions;
    private readonly IdentityLinkUserManager _identityLinkUserManager;

    private IDistributedEventBus _distributedEventBus { get; set; }

    public IdentityUserManager_Tests()
    {
        _identityUserManager = GetRequiredService<IdentityUserManager>();
        _identityUserRepository = GetRequiredService<IIdentityUserRepository>();
        _identityRoleRepository = GetRequiredService<IIdentityRoleRepository>();
        _organizationUnitRepository = GetRequiredService<IOrganizationUnitRepository>();
        _lookupNormalizer = GetRequiredService<ILookupNormalizer>();
        _testData = GetRequiredService<IdentityTestData>();
        _unitOfWorkManager = GetRequiredService<IUnitOfWorkManager>();
        _currentTenant = GetRequiredService<ICurrentTenant>();
        _identityOptions = GetRequiredService<IOptions<IdentityOptions>>();
        _identityLinkUserManager = GetRequiredService<IdentityLinkUserManager>();
    }

    protected override void AfterAddApplication(IServiceCollection services)
    {
        _distributedEventBus = Substitute.For<IDistributedEventBus>();
        services.Replace(ServiceDescriptor.Singleton(_distributedEventBus));
    }

    [Fact]
    public async Task GetByIdAsync()
    {
        var user = await _identityUserManager.GetByIdAsync(_testData.UserJohnId);

        user.ShouldNotBeNull();
        user.UserName.ShouldBe("john.nash");
    }

    [Fact]
    public async Task SetRolesAsync()
    {
        using (var uow = _unitOfWorkManager.Begin())
        {
            var user = await _identityUserRepository.FindByNormalizedUserNameAsync(
                _lookupNormalizer.NormalizeName("david")
            );

            user.ShouldNotBeNull();

            var identityResult = await _identityUserManager.SetRolesAsync(user, new List<string>()
                {
                    "moderator",
                });

            identityResult.Succeeded.ShouldBeTrue();
            user.Roles.ShouldContain(x => x.RoleId == _testData.RoleModeratorId);

            await uow.CompleteAsync();
        }
    }

    [Fact]
    public async Task SetRoles_Should_Remove_Other_Roles()
    {
        using (var uow = _unitOfWorkManager.Begin())
        {
            var roleSupporter =
                await _identityRoleRepository.FindByNormalizedNameAsync(_lookupNormalizer.NormalizeName("supporter"));
            roleSupporter.ShouldNotBeNull();

            var user = await _identityUserRepository.FindByNormalizedUserNameAsync(
                _lookupNormalizer.NormalizeName("john.nash"));
            user.ShouldNotBeNull();

            var identityResult = await _identityUserManager.SetRolesAsync(user, new List<string>()
                {
                    "admin",
                });

            identityResult.Succeeded.ShouldBeTrue();
            user.Roles.ShouldNotContain(x => x.RoleId == _testData.RoleModeratorId);
            user.Roles.ShouldNotContain(x => x.RoleId == roleSupporter.Id);

            await uow.CompleteAsync();
        }
    }

    [Fact]
    public async Task SetOrganizationUnitsAsync()
    {
        using (var uow = _unitOfWorkManager.Begin())
        {
            var user = await _identityUserRepository.FindByNormalizedUserNameAsync(
                _lookupNormalizer.NormalizeName("david"));
            user.ShouldNotBeNull();

            var ou = await _organizationUnitRepository.GetAsync(
                _lookupNormalizer.NormalizeName("OU11"));
            ou.ShouldNotBeNull();

            await _identityUserManager.SetOrganizationUnitsAsync(user, new Guid[]
            {
                    ou.Id
            });

            user = await _identityUserRepository.FindByNormalizedUserNameAsync(
                _lookupNormalizer.NormalizeName("david"));
            user.OrganizationUnits.Count.ShouldBeGreaterThan(0);
            user.OrganizationUnits.FirstOrDefault(uou => uou.OrganizationUnitId == ou.Id).ShouldNotBeNull();

            await uow.CompleteAsync();


        }
    }

    [Fact]
    public async Task AddDefaultRolesAsync_In_Same_Uow()
    {
        await _identityOptions.SetAsync();

        await CreateRandomDefaultRoleAsync();

        using (var uow = _unitOfWorkManager.Begin())
        {
            var user = CreateRandomUser();

            (await _identityUserManager.CreateAsync(user)).CheckErrors();

            user.Roles.Count.ShouldBe(0);

            await _identityUserManager.AddDefaultRolesAsync(user);

            user.Roles.Count.ShouldBeGreaterThan(0);

            foreach (var roleId in user.Roles.Select(r => r.RoleId))
            {
                var role = await _identityRoleRepository.GetAsync(roleId);
                role.IsDefault.ShouldBe(true);
            }

            await uow.CompleteAsync();

        }
    }

    [Fact]
    public async Task SetOrganizationUnits_Should_Remove()
    {
        using (var uow = _unitOfWorkManager.Begin())
        {
            var ou = await _organizationUnitRepository.GetAsync(
                _lookupNormalizer.NormalizeName("OU111"));
            ou.ShouldNotBeNull();

            var user = await _identityUserRepository.FindByNormalizedUserNameAsync(
                _lookupNormalizer.NormalizeName("john.nash"));
            user.ShouldNotBeNull();

            var ouNew = await _organizationUnitRepository.GetAsync(
                _lookupNormalizer.NormalizeName("OU2"));
            ouNew.ShouldNotBeNull();

            await _identityUserManager.SetOrganizationUnitsAsync(user, new Guid[]
            {
                    ouNew.Id
            });

            user.OrganizationUnits.ShouldNotContain(x => x.OrganizationUnitId == ou.Id);

            await uow.CompleteAsync();
        }
    }

    [Fact]
    public async Task AddDefaultRolesAsync_In_Different_Uow()
    {
        await _identityOptions.SetAsync();

        await CreateRandomDefaultRoleAsync();

        Guid userId;

        using (var uow = _unitOfWorkManager.Begin())
        {
            var user = CreateRandomUser();
            userId = user.Id;

            (await _identityUserManager.CreateAsync(user)).CheckErrors();
            user.Roles.Count.ShouldBe(0);
            await uow.CompleteAsync();
        }

        using (var uow = _unitOfWorkManager.Begin())
        {
            var user = await _identityUserManager.GetByIdAsync(userId);

            await _identityUserManager.AddDefaultRolesAsync(user);
            user.Roles.Count.ShouldBeGreaterThan(0);

            foreach (var roleId in user.Roles.Select(r => r.RoleId))
            {
                var role = await _identityRoleRepository.GetAsync(roleId);
                role.IsDefault.ShouldBe(true);
            }

            await uow.CompleteAsync();
        }
    }

    [Fact]
    public async Task ShouldPeriodicallyChangePasswordAsync_Return_False()
    {
        var user = CreateRandomUser();
        AddPeriodicallyChangePasswordSettings();

        (await _identityUserManager.CreateAsync(user)).CheckErrors();
        (await _identityUserManager.ShouldPeriodicallyChangePasswordAsync(user)).ShouldBeFalse();

        await _identityUserManager.AddPasswordAsync(user, IdentityDataSeedContributor.AdminPasswordDefaultValue);
        (await _identityUserManager.ShouldPeriodicallyChangePasswordAsync(user)).ShouldBeFalse();

        user.CreationTime = DateTime.Now;
        user.SetLastPasswordChangeTime(null);
        (await _identityUserManager.ShouldPeriodicallyChangePasswordAsync(user)).ShouldBeFalse();
    }

    [Fact]
    public async Task ShouldPeriodicallyChangePasswordAsync_Return_True()
    {
        var user = CreateRandomUser();
        AddPeriodicallyChangePasswordSettings();

        (await _identityUserManager.CreateAsync(user)).CheckErrors();
        await _identityUserManager.AddPasswordAsync(user, IdentityDataSeedContributor.AdminPasswordDefaultValue);

        user.SetLastPasswordChangeTime(DateTime.UtcNow.AddDays(-3));
        (await _identityUserManager.ShouldPeriodicallyChangePasswordAsync(user)).ShouldBeTrue();

        user.CreationTime = DateTime.Now.AddDays(-3);
        user.SetLastPasswordChangeTime(null);
        (await _identityUserManager.ShouldPeriodicallyChangePasswordAsync(user)).ShouldBeTrue();
    }

    [Fact]
    public async Task IdentityUserUserNameChangedEto_Test()
    {
        var user = CreateRandomUser();

        (await _identityUserManager.CreateAsync(user)).CheckErrors();

        await _distributedEventBus.DidNotReceive()
            .PublishAsync(Arg.Any<IdentityUserUserNameChangedEto>(), Arg.Any<bool>(), Arg.Any<bool>());

        var newUser = await _identityUserManager.FindByIdAsync(user.Id.ToString());
        newUser.ShouldNotBeNull();

        using (_currentTenant.Change(Guid.NewGuid()))
        {
            var oldUsername = newUser.UserName;
            await _identityUserManager.SetUserNameAsync(newUser, "newUserName");
            await _distributedEventBus.Received()
                .PublishAsync(
                    Arg.Is<IdentityUserUserNameChangedEto>(x =>
                        x.Id == newUser.Id && x.TenantId == newUser.TenantId && x.OldUserName == oldUsername && x.UserName == "newUserName"),
                    Arg.Any<bool>(), Arg.Any<bool>());
        }

        _distributedEventBus.ClearReceivedCalls();
        await _identityUserManager.SetUserNameAsync(newUser, newUser.UserName);
        await _distributedEventBus.DidNotReceive()
            .PublishAsync(Arg.Any<IdentityUserUserNameChangedEto>(), Arg.Any<bool>(), Arg.Any<bool>());
    }

    [Fact]
    public async Task IdentityUserEmailChangedEto_Test()
    {
        var user = CreateRandomUser();

        (await _identityUserManager.CreateAsync(user)).CheckErrors();

        await _distributedEventBus.DidNotReceive()
            .PublishAsync(Arg.Any<IdentityUserEmailChangedEto>(), Arg.Any<bool>(), Arg.Any<bool>());

        var newUser = await _identityUserManager.FindByIdAsync(user.Id.ToString());
        newUser.ShouldNotBeNull();

        using (_currentTenant.Change(Guid.NewGuid()))
        {
            var oldEmail = newUser.Email;
            await _identityUserManager.SetEmailAsync(newUser, "newEmail@abp.io");
            await _distributedEventBus.Received()
                .PublishAsync(
                    Arg.Is<IdentityUserEmailChangedEto>(x =>
                        x.Id == newUser.Id && x.TenantId == newUser.TenantId && x.OldEmail == oldEmail && x.Email == "newEmail@abp.io"),
                    Arg.Any<bool>(), Arg.Any<bool>());
        }

        _distributedEventBus.ClearReceivedCalls();
        await _identityUserManager.SetEmailAsync(newUser, newUser.Email);
        await _distributedEventBus.DidNotReceive()
            .PublishAsync(Arg.Any<IdentityUserEmailChangedEto>(), Arg.Any<bool>(), Arg.Any<bool>());
    }

    [Fact]
    public async Task DeleteAsync()
    {
        await CreateRandomDefaultRoleAsync();
        var user = CreateRandomUser();
        (await _identityUserManager.CreateAsync(user)).CheckErrors();

        var user2 = CreateRandomUser();
        (await _identityUserManager.CreateAsync(user2)).CheckErrors();

        using (var uow = _unitOfWorkManager.Begin())
        {
            user = await _identityUserManager.FindByIdAsync(user.Id.ToString());
            user.ShouldNotBeNull();

            await _identityUserManager.AddClaimAsync(user, new Claim("test", "test"));
            await _identityUserManager.AddLoginAsync(user, new UserLoginInfo("test", "test", "test"));
            await _identityUserManager.AddDefaultRolesAsync(user);
            user.SetToken("test", "test", "test");
            var ou = await _organizationUnitRepository.GetAsync(_lookupNormalizer.NormalizeName("OU11"));
            await _identityUserManager.AddToOrganizationUnitAsync(user, ou);
            await _identityLinkUserManager.LinkAsync(new IdentityLinkUserInfo(user.Id), new IdentityLinkUserInfo(user2.Id));

            await uow.CompleteAsync();
        }

        using (var uow = _unitOfWorkManager.Begin())
        {
            user = await _identityUserManager.FindByIdAsync(user.Id.ToString());
            user.ShouldNotBeNull();

            user.Claims.Count.ShouldBeGreaterThan(0);
            user.Logins.Count.ShouldBeGreaterThan(0);
            user.Roles.Count.ShouldBeGreaterThan(0);
            user.Tokens.Count.ShouldBeGreaterThan(0);
            user.OrganizationUnits.Count.ShouldBeGreaterThan(0);
            (await _identityLinkUserManager.IsLinkedAsync(new IdentityLinkUserInfo(user.Id), new IdentityLinkUserInfo(user2.Id))).ShouldBeTrue();

            await _identityUserManager.DeleteAsync(user);

            user.Claims.Count.ShouldBe(0);
            user.Logins.Count.ShouldBe(0);
            user.Roles.Count.ShouldBe(0);
            user.Tokens.Count.ShouldBe(0);
            user.OrganizationUnits.Count.ShouldBe(0);
            (await _identityLinkUserManager.IsLinkedAsync(new IdentityLinkUserInfo(user.Id), new IdentityLinkUserInfo(user2.Id))).ShouldBeFalse();

            await uow.CompleteAsync();
        }
    }

    [Fact]
    public async Task ValidateUserNameAsync()
    {
        var result = await _identityUserManager.ValidateUserNameAsync("M_y+User-001@abp.io");
        result.ShouldBeTrue();

        var user = CreateRandomUser();
        (await _identityUserManager.CreateAsync(user)).CheckErrors();

        result = await _identityUserManager.ValidateUserNameAsync(user.UserName, user.Id);
        result.ShouldBeTrue();

        result = await _identityUserManager.ValidateUserNameAsync(user.UserName);
        result.ShouldBeFalse();

        result = await _identityUserManager.ValidateUserNameAsync("无效的字符");
        result.ShouldBeFalse();
    }

    [Fact]
    public async Task GetRandomUserNameAsync()
    {
        _identityUserManager.Options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
        var username = await _identityUserManager.GetRandomUserNameAsync(15);
        username.Length.ShouldBe(15);
        username.All(c => _identityUserManager.Options.User.AllowedUserNameCharacters.Contains(c)).ShouldBeTrue();

        _identityUserManager.Options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyz0123456789";
        username = await _identityUserManager.GetRandomUserNameAsync(15);
        username.Length.ShouldBe(15);
        username.All(c => _identityUserManager.Options.User.AllowedUserNameCharacters.Contains(c)).ShouldBeTrue();

        _identityUserManager.Options.User.AllowedUserNameCharacters = "0123456789";
        username = await _identityUserManager.GetRandomUserNameAsync(15);
        username.Length.ShouldBe(15);
        username.All(c => _identityUserManager.Options.User.AllowedUserNameCharacters.Contains(c)).ShouldBeTrue();

        _identityUserManager.Options.User.AllowedUserNameCharacters = null!;
        username = await _identityUserManager.GetRandomUserNameAsync(15);
        username.Length.ShouldBe(15);
        username.All(c => "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+".Contains(c)).ShouldBeTrue();
    }

    [Fact]
    public async Task GetUserNameFromEmailAsync()
    {
        _identityUserManager.Options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyz0123456789";
        var username = await _identityUserManager.GetUserNameFromEmailAsync("Yönetici@abp.io");
        username.Length.ShouldBe("Yönetici".Length); //random username
        username.All(c => "abcdefghijklmnopqrstuvwxyz0123456789".Contains(c)).ShouldBeTrue();

        _identityUserManager.Options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyz0123456789";
        username = await _identityUserManager.GetUserNameFromEmailAsync("admin@abp.io");
        username.Length.ShouldBe(9); //admin and random 4 numbers
        username.ShouldContain("admin");
        Regex.IsMatch(username, @"\d{4}$").ShouldBeTrue();

        _identityUserManager.Options.User.AllowedUserNameCharacters = "admin01234";
        username = await _identityUserManager.GetUserNameFromEmailAsync("admin@abp.io");
        username.Length.ShouldBe(9); //admin and random 4 numbers
        username.ShouldContain("admin");
        Regex.IsMatch(username, @"[0-4]{3}$").ShouldBeTrue();

        _identityUserManager.Options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyz";
        username = await _identityUserManager.GetUserNameFromEmailAsync("admin@abp.io");
        username.Length.ShouldBe(9); //admin and random 4 characters
        username.ShouldContain("admin");
        Regex.IsMatch(username, @"[a-z]{4}$").ShouldBeTrue();

        _identityUserManager.Options.User.AllowedUserNameCharacters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        username = await _identityUserManager.GetUserNameFromEmailAsync("ADMIN@abp.io");
        username.Length.ShouldBe(9); //admin and random 4 characters
        username.ShouldContain("ADMIN");
        Regex.IsMatch(username, @"[A-Z]{4}$").ShouldBeTrue();

        _identityUserManager.Options.User.AllowedUserNameCharacters = null!;
        username = await _identityUserManager.GetUserNameFromEmailAsync("admin@abp.io");
        username.Length.ShouldBe(9); //admin and random 4 numbers
        username.ShouldContain("admin");
        Regex.IsMatch(username, @"[0-9]{4}$").ShouldBeTrue();
    }

    private async Task CreateRandomDefaultRoleAsync()
    {
        await _identityRoleRepository.InsertAsync(
            new IdentityRole(
                Guid.NewGuid(),
                Guid.NewGuid().ToString()
            )
            {
                IsDefault = true
            }
        );
    }

    private static IdentityUser CreateRandomUser()
    {
        return new IdentityUser(
            Guid.NewGuid(),
            Guid.NewGuid().ToString(),
            Guid.NewGuid().ToString() + "@abp.io"
        );
    }

    private static void AddPeriodicallyChangePasswordSettings()
    {
        TestSettingValueProvider.AddSetting(IdentitySettingNames.Password.PasswordChangePeriodDays, 2.ToString());
        TestSettingValueProvider.AddSetting(IdentitySettingNames.Password.ForceUsersToPeriodicallyChangePassword, true.ToString());
    }
}
