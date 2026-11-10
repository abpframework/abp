using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Volo.Abp.Identity;
using Volo.Abp.Identity.Settings;
using Volo.Abp.Uow;
using Xunit;

namespace Volo.Abp.Account.Web.Pages.Account;

public class Register_Tests : AbpAccountWebTestBase
{
    [Fact]
    public async Task Should_Sign_In_Registered_User()
    {
        var (result, alerts) = await RegisterAsync();

        result.ShouldNotBeOfType<PageResult>();
        alerts.ShouldBeEmpty();
        IsSignedIn().ShouldBeTrue();
    }

    [Fact]
    public async Task Should_Show_Message_If_Registered_User_Is_Not_Allowed_To_Sign_In()
    {
        SetSetting(IdentitySettingNames.SignIn.RequireConfirmedEmail, "true");

        var (result, alerts) = await RegisterAsync();

        result.ShouldBeOfType<PageResult>();
        alerts.ShouldBe(new[] { L("RegisteredButLoginIsNotAllowed") });
        IsSignedIn().ShouldBeFalse();
    }

    private async Task<(IActionResult Result, string[] Alerts)> RegisterAsync()
    {
        var userName = "user-" + Guid.NewGuid().ToString("N");

        using var scope = ServiceProvider.CreateScope();
        var page = CreatePage<RegisterModel>(scope.ServiceProvider);
        page.Input = new RegisterModel.PostInput { UserName = userName, EmailAddress = userName + "@abp.io", Password = Password };
        page.ReturnUrl = "/";

        using (var uow = GetRequiredService<IUnitOfWorkManager>().Begin())
        {
            var result = await page.OnPostAsync();
            await uow.CompleteAsync();

            (await GetRequiredService<IdentityUserManager>().FindByNameAsync(userName)).ShouldNotBeNull();
            return (result, GetAlerts(scope.ServiceProvider));
        }
    }
}

public class Register_ReplacedSignInManager_Tests : Register_Tests
{
    protected override void AfterAddApplication(IServiceCollection services)
    {
        ReplaceSignInManager(services);
    }
}
