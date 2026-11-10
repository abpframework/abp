using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Volo.Abp.Identity;
using Volo.Abp.Modularity;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Uow;
using Xunit;

namespace Volo.Abp.Account.Web.Pages.Account;

// The page has no step to change the password, so a user that has to change the password is not allowed to log in.
public abstract class Login_NotAllowedUser_TestBase<TStartupModule> : AbpAccountWebTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{
    protected virtual Guid? UserTenantId => null;

    protected override void AfterAddApplication(IServiceCollection services)
    {
        services.Configure<AbpIdentityOptions>(options =>
        {
            options.ExternalLoginProviders.Add<TestExternalLoginProvider>(TestExternalLoginProvider.Name);
        });
    }

    [Fact]
    public async Task Should_Check_Password_Of_User_That_Has_To_Change_Password()
    {
        var user = await CreateUserAsync(UserTenantId, RestrictedUserState.MustChangePassword);

        (await LoginAsync(user.UserName, "wrong-password")).ShouldBe(new[] { L("InvalidUserNameOrPassword") });
        (await LoginAsync(user.UserName, Password)).ShouldBe(new[] { L("LoginIsNotAllowed") });
    }

    [Fact]
    public async Task Should_Show_Locked_Out_Message_For_Locked_Out_User_That_Has_To_Change_Password()
    {
        var user = await CreateUserAsync(UserTenantId, RestrictedUserState.MustChangePassword, RestrictedUserState.LockedOut);

        (await LoginAsync(user.UserName, "wrong-password")).ShouldBe(new[] { L("UserLockedOutMessage") });
    }

    // The user may be authenticated by an external login provider, the local password is not the reason of the rejection.
    [Fact]
    public async Task Should_Not_Check_Password_Of_Unconfirmed_User_That_Has_To_Change_Password()
    {
        var user = await CreateUserAsync(UserTenantId, RestrictedUserState.MustChangePassword, RestrictedUserState.RequiresConfirmation);

        (await LoginAsync(user.UserName, "wrong-password")).ShouldBe(new[] { L("LoginIsNotAllowed") });
    }

    // The password of the external login provider can not be changed by the application.
    [Fact]
    public async Task Should_Not_Allow_External_Login_User_That_Has_To_Change_Password()
    {
        var user = await CreateUserAsync(UserTenantId, RestrictedUserState.MustChangePassword);

        (await LoginAsync(user.UserName, TestExternalLoginProvider.Password)).ShouldBe(new[] { L("ExternalLoginPasswordChangeRequired") });
    }

    [Fact]
    public async Task Should_Show_Locked_Out_Message_For_Locked_Out_External_Login_User_That_Has_To_Change_Password()
    {
        var user = await CreateUserAsync(UserTenantId, RestrictedUserState.MustChangePassword, RestrictedUserState.LockedOut);

        (await LoginAsync(user.UserName, TestExternalLoginProvider.Password)).ShouldBe(new[] { L("UserLockedOutMessage") });
    }

    [Fact]
    public async Task Should_Not_Allow_Unconfirmed_External_Login_User_That_Has_To_Change_Password()
    {
        var user = await CreateUserAsync(UserTenantId, RestrictedUserState.MustChangePassword, RestrictedUserState.RequiresConfirmation);

        (await LoginAsync(user.UserName, TestExternalLoginProvider.Password)).ShouldBe(new[] { L("LoginIsNotAllowed") });
    }

    private async Task<string[]> LoginAsync(string userName, string password)
    {
        using var scope = ServiceProvider.CreateScope();
        var page = CreatePage<LoginModel>(scope.ServiceProvider);
        page.LoginInput = new LoginModel.LoginInputModel { UserNameOrEmailAddress = userName, Password = password };

        using (var uow = GetRequiredService<IUnitOfWorkManager>().Begin())
        {
            await page.OnPostAsync("Login");
            await uow.CompleteAsync();
        }

        IsSignedIn().ShouldBeFalse();
        return GetAlerts(scope.ServiceProvider);
    }
}

public class Login_NotAllowedUser_Tests : Login_NotAllowedUser_TestBase<AbpAccountWebTestModule>
{
}

// The user of a tenant logs in on the host side.
public class Login_NotAllowedUser_SharedUser_Tests : Login_NotAllowedUser_Tests
{
    protected override Guid? UserTenantId { get; } = Guid.NewGuid();

    protected override void AfterAddApplication(IServiceCollection services)
    {
        base.AfterAddApplication(services);
        services.Configure<AbpMultiTenancyOptions>(options =>
        {
            options.IsEnabled = true;
            options.UserSharingStrategy = TenantUserSharingStrategy.Shared;
        });
    }
}
