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

    [Fact]
    public async Task Should_Allow_Update_Without_UserName_Or_Email_Changes()
    {
        var tenantId = Guid.NewGuid();

        using (_currentTenant.Change(tenantId))
        {
            var user = new IdentityUser(Guid.NewGuid(), "unchanged-user", "unchanged@volosoft.com") { Name = "Original" };
            (await _identityUserManager.CreateAsync(user)).Succeeded.ShouldBeTrue();

            user.Name = "Changed";
            (await _identityUserManager.UpdateAsync(user)).Succeeded.ShouldBeTrue();
        }
    }

    [Fact]
    public async Task Should_Allow_Update_Changing_UserName_To_A_Globally_Unique_Value()
    {
        var tenantId = Guid.NewGuid();

        using (_currentTenant.Change(tenantId))
        {
            var user = new IdentityUser(Guid.NewGuid(), "rename-start", "rename@volosoft.com");
            (await _identityUserManager.CreateAsync(user)).Succeeded.ShouldBeTrue();

            var result = await _identityUserManager.SetUserNameAsync(user, "rename-end");
            result.Succeeded.ShouldBeTrue();
        }
    }

    [Fact]
    public async Task Should_Allow_Update_Changing_Email_To_A_Globally_Unique_Value()
    {
        var tenantId = Guid.NewGuid();

        using (_currentTenant.Change(tenantId))
        {
            var user = new IdentityUser(Guid.NewGuid(), "email-change", "email-before@volosoft.com");
            (await _identityUserManager.CreateAsync(user)).Succeeded.ShouldBeTrue();

            var result = await _identityUserManager.SetEmailAsync(user, "email-after@volosoft.com");
            result.Succeeded.ShouldBeTrue();
        }
    }

    // Host-user scenarios (TenantId == null): still must enforce global uniqueness on Create.
    [Fact]
    public async Task Should_Reject_Duplicate_UserName_Between_Host_User_And_Tenant_User()
    {
        var tenantId = Guid.NewGuid();
        const string sharedName = "host-vs-tenant-name";

        // Host user first.
        var hostUser = new IdentityUser(Guid.NewGuid(), sharedName, "host-side@volosoft.com");
        (await _identityUserManager.CreateAsync(hostUser)).Succeeded.ShouldBeTrue();

        using (_currentTenant.Change(tenantId))
        {
            var tenantUser = new IdentityUser(Guid.NewGuid(), sharedName, "tenant-side@volosoft.com");
            var result = await _identityUserManager.CreateAsync(tenantUser);

            result.Succeeded.ShouldBeFalse();
            result.Errors.Any(e => e.Code == "DuplicateUserName").ShouldBeTrue();
        }
    }

    [Fact]
    public async Task Should_Reject_Duplicate_Email_Between_Host_User_And_Tenant_User()
    {
        var tenantId = Guid.NewGuid();
        const string sharedEmail = "host-vs-tenant-email@volosoft.com";

        var hostUser = new IdentityUser(Guid.NewGuid(), "host-email-user", sharedEmail);
        (await _identityUserManager.CreateAsync(hostUser)).Succeeded.ShouldBeTrue();

        using (_currentTenant.Change(tenantId))
        {
            var tenantUser = new IdentityUser(Guid.NewGuid(), "tenant-email-user", sharedEmail);
            var result = await _identityUserManager.CreateAsync(tenantUser);

            result.Succeeded.ShouldBeFalse();
            result.Errors.Any(e => e.Code == "DuplicateEmail").ShouldBeTrue();
        }
    }

}
