using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Volo.Abp.Identity.AspNetCore;
using Volo.Abp.Uow;
using Xunit;
using IdentityUser = Volo.Abp.Identity.IdentityUser;

namespace Volo.Abp.Account.Web.Pages.Account;

// Some pages sign in a user without asking for the credentials of the user, for example after the registration.
public abstract class AccountPageModel_SignInCheck_TestBase : AbpAccountWebTestBase
{
    [Fact]
    public async Task Should_Allow_User()
    {
        (await CheckUserSignInAsync()).ShouldBeNull();
    }

    [Theory]
    [InlineData(RestrictedUserState.Inactive)]
    [InlineData(RestrictedUserState.RequiresConfirmation)]
    [InlineData(RestrictedUserState.MustChangePassword)]
    [InlineData(RestrictedUserState.PasswordExpired)]
    public async Task Should_Not_Allow_Restricted_User(RestrictedUserState state)
    {
        (await CheckUserSignInAsync(state)).ShouldBe(SignInResult.NotAllowed);
    }

    [Fact]
    public async Task Should_Not_Allow_Locked_Out_User()
    {
        (await CheckUserSignInAsync(RestrictedUserState.LockedOut)).ShouldBe(SignInResult.LockedOut);
    }

    protected async Task<SignInResult> CheckUserSignInAsync(params RestrictedUserState[] states)
    {
        var user = await CreateUserAsync(states: states);

        using var scope = ServiceProvider.CreateScope();
        var page = CreatePage<TestAccountPageModel>(scope.ServiceProvider);
        using (var uow = GetRequiredService<IUnitOfWorkManager>().Begin())
        {
            var result = await page.CheckAsync(user);
            await uow.CompleteAsync();
            return result;
        }
    }
}

public class AccountPageModel_SignInCheck_Tests : AccountPageModel_SignInCheck_TestBase
{
    [Fact]
    public void Should_Use_AbpSignInManager()
    {
        GetRequiredService<SignInManager<IdentityUser>>().ShouldBeAssignableTo<AbpSignInManager>();
    }
}

public class AccountPageModel_SignInCheck_ReplacedSignInManager_Tests : AccountPageModel_SignInCheck_TestBase
{
    protected override void AfterAddApplication(IServiceCollection services)
    {
        ReplaceSignInManager(services);
    }

    [Fact]
    public void Should_Use_Replaced_SignInManager()
    {
        GetRequiredService<SignInManager<IdentityUser>>().ShouldBeOfType<TestSignInManager>();
    }
}

public class TestAccountPageModel : AccountPageModel
{
    public Task<SignInResult> CheckAsync(IdentityUser user)
    {
        return CheckUserSignInAsync(user);
    }
}
