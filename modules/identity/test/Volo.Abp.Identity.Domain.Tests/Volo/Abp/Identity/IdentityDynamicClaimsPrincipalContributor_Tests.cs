using System.Security.Claims;
using System.Threading.Tasks;
using Shouldly;
using Volo.Abp.Security.Claims;
using Xunit;

namespace Volo.Abp.Identity;

public class IdentityDynamicClaimsPrincipalContributor_Tests : AbpIdentityDomainTestBase
{
    private readonly IdentityUserManager _identityUserManager;
    private readonly IAbpClaimsPrincipalFactory _abpClaimsPrincipalFactory;
    private readonly AbpUserClaimsPrincipalFactory _abpUserClaimsPrincipalFactory;
    private readonly IdentityTestData _testData;

    public IdentityDynamicClaimsPrincipalContributor_Tests()
    {
        _identityUserManager = GetRequiredService<IdentityUserManager>();
        _abpClaimsPrincipalFactory = GetRequiredService<IAbpClaimsPrincipalFactory>();
        _abpUserClaimsPrincipalFactory = GetRequiredService<AbpUserClaimsPrincipalFactory>();
        _testData = GetRequiredService<IdentityTestData>();
    }

    [Fact]
    public async Task Should_Get_Correct_Claims_After_User_Updating()
    {
        IdentityUser user = null;
        ClaimsPrincipal claimsPrincipal = null;
        string securityStamp = null;
        await UsingUowAsync(async () =>
        {
            user = await _identityUserManager.GetByIdAsync(_testData.UserJohnId);
            user.ShouldNotBeNull();
            securityStamp = user.SecurityStamp;
            claimsPrincipal = await _abpUserClaimsPrincipalFactory.CreateAsync(user);

            claimsPrincipal.Claims.ShouldContain(x => x.Type == ClaimTypes.NameIdentifier && x.Value == user.Id.ToString());
            claimsPrincipal.Claims.ShouldContain(x => x.Type == ClaimTypes.Name && x.Value == user.UserName);
            claimsPrincipal.Claims.ShouldContain(x => x.Type == ClaimTypes.Email && x.Value == user.Email);
            claimsPrincipal.Claims.ShouldContain(x => x.Type == "AspNet.Identity.SecurityStamp" && x.Value == securityStamp);

            var dynamicClaimsPrincipal = await _abpClaimsPrincipalFactory.CreateDynamicAsync(claimsPrincipal);
            dynamicClaimsPrincipal.Claims.ShouldContain(x => x.Type == ClaimTypes.NameIdentifier && x.Value == user.Id.ToString());
            dynamicClaimsPrincipal.Claims.ShouldContain(x => x.Type == ClaimTypes.Name && x.Value == user.UserName);
            dynamicClaimsPrincipal.Claims.ShouldContain(x => x.Type == ClaimTypes.Email && x.Value == user.Email);
            dynamicClaimsPrincipal.Claims.ShouldContain(x => x.Type == "AspNet.Identity.SecurityStamp" && x.Value == securityStamp);//SecurityStamp is not dynamic claim

            await _identityUserManager.SetUserNameAsync(user, "newUserName");
            await _identityUserManager.SetEmailAsync(user, "newUserEmail@abp.io");
            await _identityUserManager.UpdateSecurityStampAsync(user);
        });

        var dynamicClaimsPrincipal = await _abpClaimsPrincipalFactory.CreateDynamicAsync(claimsPrincipal);
        dynamicClaimsPrincipal.Claims.ShouldContain(x => x.Type == ClaimTypes.NameIdentifier && x.Value == user.Id.ToString());
        dynamicClaimsPrincipal.Claims.ShouldContain(x => x.Type == ClaimTypes.Name && x.Value =="newUserName");
        dynamicClaimsPrincipal.Claims.ShouldContain(x => x.Type == ClaimTypes.Email && x.Value == "newUserEmail@abp.io");
        dynamicClaimsPrincipal.Claims.ShouldContain(x => x.Type == "AspNet.Identity.SecurityStamp" && x.Value == securityStamp);//SecurityStamp is not dynamic claim
    }
}
