using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Localization;
using Shouldly;
using Volo.Abp.Identity.Localization;
using Volo.Abp.MultiTenancy;
using Xunit;

namespace Volo.Abp.Identity.AspNetCore;

public class AbpIdentityUserValidator_Tests : AbpIdentityAspNetCoreTestBase
{
    private readonly IdentityUserManager _identityUserManager;
    private readonly IStringLocalizer<IdentityResource> Localizer;

    public AbpIdentityUserValidator_Tests()
    {
        _identityUserManager = GetRequiredService<IdentityUserManager>();
        Localizer = GetRequiredService<IStringLocalizer<IdentityResource>>();
    }

    [Fact]
    public async Task InvalidUserName_Messages_Test()
    {
        var user = new IdentityUser(Guid.NewGuid(), "abp 123", "user@volosoft.com");
        var identityResult = await _identityUserManager.CreateAsync(user);
        identityResult.Succeeded.ShouldBeFalse();
        identityResult.Errors.Count().ShouldBe(1);
        identityResult.Errors.First().Code.ShouldBe("InvalidUserName");
        identityResult.Errors.First().Description.ShouldBe(Localizer["Volo.Abp.Identity:InvalidUserName", "abp 123"]);
    }

    [Fact]
    public async Task Can_Not_Use_Another_Users_Email_As_Your_Username_Test()
    {
        var user1 = new IdentityUser(Guid.NewGuid(), "user1", "user1@volosoft.com");
        var identityResult = await _identityUserManager.CreateAsync(user1);
        identityResult.Succeeded.ShouldBeTrue();

        var user2 = new IdentityUser(Guid.NewGuid(), "user1@volosoft.com", "user2@volosoft.com");
        identityResult = await _identityUserManager.CreateAsync(user2);
        identityResult.Succeeded.ShouldBeFalse();
        identityResult.Errors.Count().ShouldBe(1);
        identityResult.Errors.First().Code.ShouldBe("InvalidUserName");
        identityResult.Errors.First().Description.ShouldBe(Localizer["Volo.Abp.Identity:InvalidUserName", "user1@volosoft.com"]);
    }

    [Fact]
    public async Task Can_Not_Use_Another_Users_Name_As_Your_Email_Test()
    {
        var user1 = new IdentityUser(Guid.NewGuid(), "user1@volosoft.com", "user@volosoft.com");
        var identityResult = await _identityUserManager.CreateAsync(user1);
        identityResult.Succeeded.ShouldBeTrue();

        var user2 = new IdentityUser(Guid.NewGuid(), "user2", "user1@volosoft.com");
        identityResult = await _identityUserManager.CreateAsync(user2);
        identityResult.Succeeded.ShouldBeFalse();
        identityResult.Errors.Count().ShouldBe(1);
        identityResult.Errors.First().Code.ShouldBe("InvalidEmail");
        identityResult.Errors.First().Description.ShouldBe(Localizer["Volo.Abp.Identity:InvalidEmail", "user1@volosoft.com"]);
    }
}

public class AbpIdentityUserValidator_SharedUser_Compatible_Tests : AbpIdentityUserValidator_Tests
{
    protected override void ConfigureServices(HostBuilderContext context, IServiceCollection services)
    {
        services.Configure<AbpMultiTenancyOptions>(options =>
        {
            options.IsEnabled = true;
            options.UserSharingStrategy = TenantUserSharingStrategy.Shared;
        });
    }
}

public class AbpIdentityUserValidator_SharedUser_Tests : AbpIdentityAspNetCoreTestBase
{
    private readonly IdentityUserManager _identityUserManager;
    private readonly ICurrentTenant _currentTenant;

    public AbpIdentityUserValidator_SharedUser_Tests()
    {
        _identityUserManager = GetRequiredService<IdentityUserManager>();
        _currentTenant = GetRequiredService<ICurrentTenant>();
    }

    protected override void ConfigureServices(HostBuilderContext context, IServiceCollection services)
    {
        services.Configure<AbpMultiTenancyOptions>(options =>
        {
            options.IsEnabled = true;
            options.UserSharingStrategy = TenantUserSharingStrategy.Shared;
        });
    }

    [Fact]
    public async Task Should_Reject_Duplicate_UserName_Across_Tenants()
    {
        var tenant1Id = Guid.NewGuid();
        var tenant2Id = Guid.NewGuid();

        using (_currentTenant.Change(tenant1Id))
        {
            var user1 = new IdentityUser(Guid.NewGuid(), "shared-user", "shared-user-1@volosoft.com");
            (await _identityUserManager.CreateAsync(user1)).Succeeded.ShouldBeTrue();
        }

        using (_currentTenant.Change(tenant2Id))
        {
            var user2 = new IdentityUser(Guid.NewGuid(), "shared-user", "shared-user-2@volosoft.com");
            var result = await _identityUserManager.CreateAsync(user2);

            result.Succeeded.ShouldBeFalse();
            result.Errors.Count().ShouldBe(1);
            result.Errors.First().Code.ShouldBe("DuplicateUserName");
        }
    }

    [Fact]
    public async Task Should_Reject_Duplicate_Email_Across_Tenants()
    {
        var tenant1Id = Guid.NewGuid();
        var tenant2Id = Guid.NewGuid();
        const string sharedEmail = "shared-email@volosoft.com";

        using (_currentTenant.Change(tenant1Id))
        {
            var user1 = new IdentityUser(Guid.NewGuid(), "shared-email-user-1", sharedEmail);
            (await _identityUserManager.CreateAsync(user1)).Succeeded.ShouldBeTrue();
        }

        using (_currentTenant.Change(tenant2Id))
        {
            var user2 = new IdentityUser(Guid.NewGuid(), "shared-email-user-2", sharedEmail);
            var result = await _identityUserManager.CreateAsync(user2);

            result.Succeeded.ShouldBeFalse();
            result.Errors.Count().ShouldBe(1);
            result.Errors.First().Code.ShouldBe("DuplicateEmail");
        }
    }

    [Fact]
    public async Task Should_Reject_UserName_That_Matches_Another_Users_Email_Across_Tenants()
    {
        var tenant1Id = Guid.NewGuid();
        var tenant2Id = Guid.NewGuid();
        const string sharedValue = "conflict@volosoft.com";

        using (_currentTenant.Change(tenant1Id))
        {
            var user1 = new IdentityUser(Guid.NewGuid(), "unique-user", sharedValue);
            (await _identityUserManager.CreateAsync(user1)).Succeeded.ShouldBeTrue();
        }

        using (_currentTenant.Change(tenant2Id))
        {
            var user2 = new IdentityUser(Guid.NewGuid(), sharedValue, "another@volosoft.com");
            var result = await _identityUserManager.CreateAsync(user2);

            result.Succeeded.ShouldBeFalse();
            result.Errors.Count().ShouldBe(1);
            result.Errors.First().Code.ShouldBe("InvalidUserName");
        }
    }

    [Fact]
    public async Task Should_Reject_Email_That_Matches_Another_Users_UserName_Across_Tenants()
    {
        var tenant1Id = Guid.NewGuid();
        var tenant2Id = Guid.NewGuid();
        const string sharedValue = "conflict-user";

        using (_currentTenant.Change(tenant1Id))
        {
            var user1 = new IdentityUser(Guid.NewGuid(), sharedValue, "conflict-user-1@volosoft.com");
            (await _identityUserManager.CreateAsync(user1)).Succeeded.ShouldBeTrue();
        }

        using (_currentTenant.Change(tenant2Id))
        {
            var user2 = new IdentityUser(Guid.NewGuid(), "another-user", sharedValue);
            var result = await _identityUserManager.CreateAsync(user2);

            result.Succeeded.ShouldBeFalse();
            result.Errors.Count().ShouldBe(1);
            result.Errors.First().Code.ShouldBe("InvalidEmail");
        }
    }
}
