using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Shouldly;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Uow;
using Xunit;

namespace Volo.Abp.Identity.AspNetCore;

public class Shared_SignIn_Tests : SharedAbpIdentityAspNetCoreTestBase
{
    [Fact]
    public async Task PasswordSignInAsync_Should_Sign_In_Tenant_User_From_Host_Context()
    {
        // In shared mode, calling PasswordSignInAsync with a username while CurrentTenant is host
        // must resolve the tenant user via FindSharedUserByNameAsync, apply the user's tenant
        // IdentityOptions (the fix added in AbpSignInManager), and complete the sign-in.
        var userManager = GetRequiredService<IdentityUserManager>();
        var currentTenant = GetRequiredService<ICurrentTenant>();
        var unitOfWorkManager = GetRequiredService<IUnitOfWorkManager>();

        var tenantId = Guid.NewGuid();
        const string userName = "shared-password-signin";
        const string password = "Shared!9Aa";

        using (var uow = unitOfWorkManager.Begin())
        {
            using (currentTenant.Change(tenantId))
            {
                var user = new IdentityUser(Guid.NewGuid(), userName, userName + "@abp.io", tenantId);
                (await userManager.CreateAsync(user, password)).Succeeded.ShouldBeTrue();
            }
            await uow.CompleteAsync();
        }

        var response = await Client.GetAsync($"/api/signin-test/password?userName={userName}&password={password}");
        response.EnsureSuccessStatusCode();
        var result = await response.Content.ReadAsStringAsync();

        result.ShouldBe("Succeeded");
    }

    [Fact]
    public async Task PasswordSignInAsync_Should_Fail_For_Wrong_Password_In_Shared_Mode()
    {
        var userManager = GetRequiredService<IdentityUserManager>();
        var currentTenant = GetRequiredService<ICurrentTenant>();
        var unitOfWorkManager = GetRequiredService<IUnitOfWorkManager>();

        var tenantId = Guid.NewGuid();
        const string userName = "shared-password-wrong";

        using (var uow = unitOfWorkManager.Begin())
        {
            using (currentTenant.Change(tenantId))
            {
                var user = new IdentityUser(Guid.NewGuid(), userName, userName + "@abp.io", tenantId);
                (await userManager.CreateAsync(user, "Shared!9Aa")).Succeeded.ShouldBeTrue();
            }
            await uow.CompleteAsync();
        }

        var response = await Client.GetAsync($"/api/signin-test/password?userName={userName}&password=wrong");
        response.EnsureSuccessStatusCode();
        var result = await response.Content.ReadAsStringAsync();

        result.ShouldBe("Failed");
    }

    [Fact]
    public async Task ExternalLoginSignInAsync_Should_Sign_In_Tenant_User_From_Host_Context()
    {
        // Covers the AbpSignInManager.ExternalLoginSignInAsync override: finds the user via
        // FindSharedUserByLoginAsync, switches CurrentTenant to user.TenantId, applies tenant
        // IdentityOptions, then calls PreSignInCheck + SignInOrTwoFactorAsync.
        const string loginProvider = "test-provider";
        var providerKey = "ext-" + Guid.NewGuid().ToString("N").Substring(0, 8);

        var userManager = GetRequiredService<IdentityUserManager>();
        var currentTenant = GetRequiredService<ICurrentTenant>();
        var unitOfWorkManager = GetRequiredService<IUnitOfWorkManager>();

        var tenantId = Guid.NewGuid();

        using (var uow = unitOfWorkManager.Begin())
        {
            using (currentTenant.Change(tenantId))
            {
                var user = new IdentityUser(Guid.NewGuid(), "shared-external-signin", "shared-external-signin@abp.io", tenantId);
                (await userManager.CreateAsync(user, "Shared!9Aa")).Succeeded.ShouldBeTrue();
                (await userManager.AddLoginAsync(user, new UserLoginInfo(loginProvider, providerKey, "Test Provider"))).Succeeded.ShouldBeTrue();
            }
            await uow.CompleteAsync();
        }

        var response = await Client.GetAsync($"/api/signin-test/external-login-signin?loginProvider={loginProvider}&providerKey={providerKey}");
        response.EnsureSuccessStatusCode();
        var result = await response.Content.ReadAsStringAsync();

        result.ShouldBe("Succeeded");
    }

    [Fact]
    public async Task ExternalLoginSignInAsync_Should_Fail_For_Unknown_Provider_Key_In_Shared_Mode()
    {
        var response = await Client.GetAsync($"/api/signin-test/external-login-signin?loginProvider=unknown&providerKey=none");
        response.EnsureSuccessStatusCode();
        var result = await response.Content.ReadAsStringAsync();

        result.ShouldBe("Failed");
    }
}
