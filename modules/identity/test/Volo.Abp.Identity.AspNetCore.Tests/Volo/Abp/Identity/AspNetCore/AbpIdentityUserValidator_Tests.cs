using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Localization;
using Shouldly;
using Volo.Abp.Identity.Localization;
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
        identityResult.Errors.First().Description.ShouldBe(Localizer["InvalidUserName", "user1@volosoft.com"]);
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
