using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Identity;
using Xunit;

namespace Volo.Abp.OpenIddict.Integration;

public class OpenIddictPasswordGrant_Integration_Tests : OpenIddictPasswordGrantIntegrationTestBase
{
    [Fact]
    public async Task Password_Grant_Without_TwoFactor_Should_Issue_Token()
    {
        await WithUnitOfWorkAsync(async serviceProvider =>
        {
            var userManager = serviceProvider.GetRequiredService<IdentityUserManager>();
            var user = await userManager.FindByNameAsync(OpenIddictPasswordGrantTestData.UserName);
            (await userManager.SetTwoFactorEnabledAsync(user, false)).CheckErrors();
        });

        var response = await RequestPasswordTokenAsync();

        await AssertAccessTokenAsync(response);
    }
}
