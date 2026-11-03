using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Volo.Abp.Identity;
using Volo.Abp.Uow;
using Xunit;
using IdentityUser = Volo.Abp.Identity.IdentityUser;

namespace Volo.Abp.Account.Web.Pages.Account;

// The external login is not linked to a user yet, the user is found by the email of the external login.
public class Login_ExternalLoginCallback_Tests : AbpAccountWebTestBase
{
    [Fact]
    public async Task Should_Sign_In_User_That_Is_Found_By_Email()
    {
        var user = await CreateUserAsync();
        var providerKey = Guid.NewGuid().ToString("N");

        await CallbackAsync(user, providerKey);

        IsSignedIn().ShouldBeTrue();
        (await FindByLoginAsync(providerKey))?.Id.ShouldBe(user.Id);
    }

    [Theory]
    [InlineData(RestrictedUserState.Inactive)]
    [InlineData(RestrictedUserState.LockedOut)]
    [InlineData(RestrictedUserState.RequiresConfirmation)]
    [InlineData(RestrictedUserState.MustChangePassword)]
    [InlineData(RestrictedUserState.PasswordExpired)]
    public async Task Should_Not_Sign_In_Restricted_User_That_Is_Found_By_Email(RestrictedUserState state)
    {
        var user = await CreateUserAsync(states: state);
        var providerKey = Guid.NewGuid().ToString("N");

        await Should.ThrowAsync<UserFriendlyException>(() => CallbackAsync(user, providerKey));

        IsSignedIn().ShouldBeFalse();
        (await FindByLoginAsync(providerKey)).ShouldBeNull();
    }

    private async Task CallbackAsync(IdentityUser user, string providerKey)
    {
        var principal = new ClaimsPrincipal(new ClaimsIdentity(new[]
        {
            new Claim(ClaimTypes.NameIdentifier, providerKey),
            new Claim(ClaimTypes.Email, user.Email)
        }, IdentityConstants.ExternalScheme));

        using var scope = ServiceProvider.CreateScope();
        var page = CreatePage<LoginModel>(scope.ServiceProvider, principal);
        using (var uow = GetRequiredService<IUnitOfWorkManager>().Begin())
        {
            await page.OnGetExternalLoginCallbackAsync("/");
            await uow.CompleteAsync();
        }
    }

    private async Task<IdentityUser> FindByLoginAsync(string providerKey)
    {
        using (var uow = GetRequiredService<IUnitOfWorkManager>().Begin())
        {
            return await GetRequiredService<IdentityUserManager>().FindByLoginAsync(ExternalLoginProvider, providerKey);
        }
    }
}

public class Login_ExternalLoginCallback_ReplacedSignInManager_Tests : Login_ExternalLoginCallback_Tests
{
    protected override void AfterAddApplication(IServiceCollection services)
    {
        ReplaceSignInManager(services);
    }
}
