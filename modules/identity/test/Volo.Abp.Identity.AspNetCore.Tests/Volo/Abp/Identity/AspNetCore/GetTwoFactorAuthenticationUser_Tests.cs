using System;
using System.Linq;
using System.Threading.Tasks;
using Shouldly;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Uow;
using Xunit;

namespace Volo.Abp.Identity.AspNetCore;

public class GetTwoFactorAuthenticationUser_Tests : SharedAbpIdentityAspNetCoreTestBase
{
    [Fact]
    public async Task Should_Resolve_Tenant_User_By_Id_When_Current_Tenant_Is_Host()
    {
        var userRepository = GetRequiredService<IIdentityUserRepository>();
        var currentTenant = GetRequiredService<ICurrentTenant>();
        var unitOfWorkManager = GetRequiredService<IUnitOfWorkManager>();

        var tenantId = Guid.NewGuid();
        Guid tenantUserId;

        using (var uow = unitOfWorkManager.Begin())
        {
            using (currentTenant.Change(tenantId))
            {
                var user = new IdentityUser(Guid.NewGuid(), "shared-2fa-tenant-user", "shared-2fa-tenant-user@abp.io", tenantId);
                await userRepository.InsertAsync(user);
                tenantUserId = user.Id;
            }
            await uow.CompleteAsync();
        }

        await WriteTwoFactorCookieAsync(tenantUserId);

        var getResponse = await Client.GetAsync("/api/signin-test/get-two-factor-user");
        getResponse.EnsureSuccessStatusCode();
        var content = await getResponse.Content.ReadAsStringAsync();

        content.ShouldBe(tenantUserId.ToString());
    }

    [Fact]
    public async Task Should_Return_Null_When_No_Two_Factor_Cookie()
    {
        var getResponse = await Client.GetAsync("/api/signin-test/get-two-factor-user");
        getResponse.EnsureSuccessStatusCode();
        var content = await getResponse.Content.ReadAsStringAsync();

        content.ShouldBe("null");
    }

    [Fact]
    public async Task TwoFactorSignInAsync_Should_Resolve_Tenant_User_In_Shared_Mode()
    {
        // If the tenant switch inside AbpSignInManager.TwoFactorSignInAsync regresses,
        // the base implementation would not find the user and return SignInResult.Failed.
        // An inactive tenant user lets PreSignInCheck return NotAllowed, proving the user
        // was actually located and inspected under its own tenant context.
        var tenantUserId = await CreateInactiveTenantUserAsync("shared-2fa-signin", "shared-2fa-signin@abp.io");

        await WriteTwoFactorCookieAsync(tenantUserId);

        var response = await Client.GetAsync("/api/signin-test/two-factor-signin?provider=Email&code=invalid");
        response.EnsureSuccessStatusCode();
        var result = await response.Content.ReadAsStringAsync();

        result.ShouldBe("NotAllowed");
    }

    [Fact]
    public async Task TwoFactorRecoveryCodeSignInAsync_Should_Resolve_Tenant_User_In_Shared_Mode()
    {
        // AbpSignInManager.TwoFactorRecoveryCodeSignInAsync runs PreSignInCheck after locating
        // the user under its own tenant context. An inactive tenant user therefore produces
        // NotAllowed only when the override actually resolves the user; a regression would
        // return Failed instead.
        var tenantUserId = await CreateInactiveTenantUserAsync("shared-2fa-recovery", "shared-2fa-recovery@abp.io");

        await WriteTwoFactorCookieAsync(tenantUserId);

        var response = await Client.GetAsync("/api/signin-test/two-factor-recovery-signin?recoveryCode=invalid");
        response.EnsureSuccessStatusCode();
        var result = await response.Content.ReadAsStringAsync();

        result.ShouldBe("NotAllowed");
    }

    private async Task<Guid> CreateInactiveTenantUserAsync(string userName, string email)
    {
        return await CreateTenantUserAsync(userName, email, isActive: false);
    }

    private async Task<Guid> CreateTenantUserAsync(string userName, string email, bool isActive)
    {
        var userRepository = GetRequiredService<IIdentityUserRepository>();
        var currentTenant = GetRequiredService<ICurrentTenant>();
        var unitOfWorkManager = GetRequiredService<IUnitOfWorkManager>();

        var tenantId = Guid.NewGuid();
        Guid userId;

        using (var uow = unitOfWorkManager.Begin())
        {
            using (currentTenant.Change(tenantId))
            {
                var user = new IdentityUser(Guid.NewGuid(), userName, email, tenantId);
                if (!isActive)
                {
                    user.SetIsActive(false);
                }
                await userRepository.InsertAsync(user);
                userId = user.Id;
            }
            await uow.CompleteAsync();
        }

        return userId;
    }

    private async Task WriteTwoFactorCookieAsync(Guid userId)
    {
        var writeResponse = await Client.GetAsync($"/api/signin-test/write-two-factor-cookie?userId={userId}");
        writeResponse.EnsureSuccessStatusCode();

        if (writeResponse.Headers.TryGetValues("Set-Cookie", out var setCookies))
        {
            Client.DefaultRequestHeaders.Remove("Cookie");
            foreach (var cookie in setCookies)
            {
                Client.DefaultRequestHeaders.Add("Cookie", cookie.Split(';').First());
            }
        }
    }
}
