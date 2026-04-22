using System;
using System.Linq;
using System.Threading.Tasks;
using Shouldly;
using Volo.Abp.Domain.Repositories;
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

        var writeResponse = await Client.GetAsync($"/api/signin-test/write-two-factor-cookie?userId={tenantUserId}");
        writeResponse.EnsureSuccessStatusCode();

        if (writeResponse.Headers.TryGetValues("Set-Cookie", out var setCookies))
        {
            foreach (var cookie in setCookies)
            {
                Client.DefaultRequestHeaders.Add("Cookie", cookie.Split(';').First());
            }
        }

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
}
