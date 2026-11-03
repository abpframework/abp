using System;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Volo.Abp.MultiTenancy;
using Xunit;

namespace Volo.Abp.Account.Web.Pages.Account;

// The login page is replaced by the login page of IdentityServer.
public class IdentityServerSupportedLogin_NotAllowedUser_Tests : Login_NotAllowedUser_TestBase<AbpAccountWebIdentityServerTestModule>
{
    [Fact]
    public void Should_Use_Login_Page_Of_IdentityServer()
    {
        using var scope = ServiceProvider.CreateScope();
        CreatePage<LoginModel>(scope.ServiceProvider).ShouldBeOfType<IdentityServerSupportedLoginModel>();
    }
}

// The user of a tenant logs in on the host side.
public class IdentityServerSupportedLogin_NotAllowedUser_SharedUser_Tests : IdentityServerSupportedLogin_NotAllowedUser_Tests
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
