using System;
using System.Linq;
using System.Threading.Tasks;
using Shouldly;
using Volo.Abp.Uow;
using Xunit;

namespace Volo.Abp.Identity.AspNetCore;

public class Isolated_TwoFactor_Tests : AbpIdentityAspNetCoreTestBase
{
    [Fact]
    public async Task TwoFactorRecoveryCodeSignInAsync_Should_Return_NotAllowed_For_Inactive_User()
    {
        // The AbpSignInManager override adds PreSignInCheck to the recovery-code path (the base
        // AspNetCore Identity implementation does not). This test asserts that behavior also works
        // in the default (isolated) configuration so the new invariant is protected across modes.
        var userManager = GetRequiredService<IdentityUserManager>();
        var userRepository = GetRequiredService<IIdentityUserRepository>();
        var unitOfWorkManager = GetRequiredService<IUnitOfWorkManager>();

        Guid userId;
        using (var uow = unitOfWorkManager.Begin())
        {
            var user = new IdentityUser(Guid.NewGuid(), "iso-recovery-inactive", "iso-recovery-inactive@abp.io");
            (await userManager.CreateAsync(user, "Iso!9Aa")).Succeeded.ShouldBeTrue();
            user.SetIsActive(false);
            await userRepository.UpdateAsync(user);
            userId = user.Id;
            await uow.CompleteAsync();
        }

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

        var response = await Client.GetAsync("/api/signin-test/two-factor-recovery-signin?recoveryCode=invalid");
        response.EnsureSuccessStatusCode();
        var result = await response.Content.ReadAsStringAsync();

        result.ShouldBe("NotAllowed");
    }
}
