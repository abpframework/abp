using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using Shouldly;
using Volo.Abp.Account.Web;
using Volo.Abp.Account.Web.Pages.Account;
using Volo.Abp.Caching;
using Volo.Abp.Data;
using Volo.Abp.Identity;
using Volo.Abp.Identity.AspNetCore;
using Volo.Abp.Modularity;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Security.Claims;
using Volo.Abp.Testing;
using Volo.Abp.Uow;
using Xunit;
using IdentityUser = Volo.Abp.Identity.IdentityUser;

namespace Volo.Abp.Account;

[DependsOn(typeof(AbpAccountApplicationTestModule), typeof(AbpAccountWebModule))]
public class AccountWebTestModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddSingleton(Substitute.For<IWebHostEnvironment>());
        Configure<AbpUnitOfWorkDefaultOptions>(options => options.TransactionBehavior = UnitOfWorkTransactionBehavior.Disabled);
    }
}

public class ExternalLoginSecurityLog_Tests : AbpIntegratedTest<AccountWebTestModule>
{
    protected override void SetAbpApplicationCreationOptions(AbpApplicationCreationOptions options)
    {
        options.UseAutofac();
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public async Task Callback_Should_Record_One_Success_With_The_Actual_Username(bool alreadyLinked)
    {
        using var scope = ServiceProvider.CreateScope();
        var services = scope.ServiceProvider;
        var manager = services.GetRequiredService<IdentityUserManager>();
        var user = new IdentityUser(Guid.NewGuid(), "external-user", "external-user@example.test")
        {
            Name = "Different display name"
        };
        using (var uow = services.GetRequiredService<IUnitOfWorkManager>().Begin())
        {
            (await manager.CreateAsync(user)).CheckErrors();
            if (alreadyLinked)
            {
                (await manager.AddLoginAsync(user, new UserLoginInfo("Example", "key", "Example"))).CheckErrors();
            }
            await uow.CompleteAsync();
        }

        var (page, authentication) = CreatePage(services, user.Email);
        using (var uow = services.GetRequiredService<IUnitOfWorkManager>().Begin())
        {
            await page.OnGetExternalLoginCallbackAsync("/");
            await uow.CompleteAsync();
        }

        await authentication.Received(1).SignInAsync(page.HttpContext, IdentityConstants.ApplicationScheme,
            Arg.Is<ClaimsPrincipal>(principal => principal.FindFirst(AbpClaimTypes.UserId).Value == user.Id.ToString()),
            Arg.Any<AuthenticationProperties>());
        using (var uow = services.GetRequiredService<IUnitOfWorkManager>().Begin())
        {
            (await manager.FindByLoginAsync("Example", "key")).Id.ShouldBe(user.Id);
            var logs = await services.GetRequiredService<IIdentitySecurityLogRepository>()
                .GetListAsync(identity: IdentitySecurityLogIdentityConsts.IdentityExternal);
            var log = logs.ShouldHaveSingleItem();
            log.Action.ShouldBe(IdentitySecurityLogActionConsts.LoginSucceeded);
            log.UserName.ShouldBe(user.UserName);
            log.UserId.ShouldBe(user.Id);
            log.TenantId.ShouldBeNull();
            await uow.CompleteAsync();
        }
    }

    [Theory]
    [InlineData(true, true)]
    [InlineData(false, true)]
    [InlineData(true, false)]
    [InlineData(false, false)]
    public async Task Callback_Should_Record_Terminal_Failure_Without_Signing_In(bool lockedOut, bool alreadyLinked)
    {
        using var scope = ServiceProvider.CreateScope();
        var services = scope.ServiceProvider;
        var manager = services.GetRequiredService<IdentityUserManager>();
        var user = new IdentityUser(Guid.NewGuid(), "blocked-user", "blocked@example.test");
        using (var uow = services.GetRequiredService<IUnitOfWorkManager>().Begin())
        {
            (await manager.CreateAsync(user)).CheckErrors();
            if (alreadyLinked)
            {
                (await manager.AddLoginAsync(user, new UserLoginInfo("Example", "key", "Example"))).CheckErrors();
            }
            if (lockedOut)
            {
                (await manager.SetLockoutEnabledAsync(user, true)).CheckErrors();
                (await manager.SetLockoutEndDateAsync(user, DateTimeOffset.UtcNow.AddHours(1))).CheckErrors();
            }
            else
            {
                user.SetIsActive(false);
                (await manager.UpdateAsync(user)).CheckErrors();
            }
            await uow.CompleteAsync();
        }

        var (page, authentication) = CreatePage(services, user.Email);
        using (var uow = services.GetRequiredService<IUnitOfWorkManager>().Begin())
        {
            await Should.ThrowAsync<UserFriendlyException>(() => page.OnGetExternalLoginCallbackAsync());
            await uow.CompleteAsync();
        }
        await authentication.DidNotReceive().SignInAsync(Arg.Any<HttpContext>(),
            IdentityConstants.ApplicationScheme, Arg.Any<ClaimsPrincipal>(), Arg.Any<AuthenticationProperties>());
        var log = (await services.GetRequiredService<IIdentitySecurityLogRepository>()
            .GetListAsync(identity: IdentitySecurityLogIdentityConsts.IdentityExternal)).ShouldHaveSingleItem();
        log.Action.ShouldBe(lockedOut ? IdentitySecurityLogActionConsts.LoginLockedout : IdentitySecurityLogActionConsts.LoginNotAllowed);
        log.UserName.ShouldBe(user.UserName);
        log.UserId.ShouldBe(user.Id);
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public async Task Callback_Should_Log_The_Target_Account_When_Another_User_Is_Signed_In(bool alreadyLinked)
    {
        using var scope = ServiceProvider.CreateScope();
        var services = scope.ServiceProvider;
        var manager = services.GetRequiredService<IdentityUserManager>();
        var signedIn = new IdentityUser(Guid.NewGuid(), "signed-in-user", "signed-in@example.test");
        var target = new IdentityUser(Guid.NewGuid(), "target-user", "target@example.test");
        using (var uow = services.GetRequiredService<IUnitOfWorkManager>().Begin())
        {
            (await manager.CreateAsync(signedIn)).CheckErrors();
            (await manager.CreateAsync(target)).CheckErrors();
            if (alreadyLinked)
            {
                (await manager.AddLoginAsync(target, new UserLoginInfo("Example", "key", "Example"))).CheckErrors();
            }
            target.SetIsActive(false);
            (await manager.UpdateAsync(target)).CheckErrors();
            await uow.CompleteAsync();
        }

        var (page, _) = CreatePage(services, target.Email);
        page.HttpContext.User = await services.GetRequiredService<IUserClaimsPrincipalFactory<IdentityUser>>().CreateAsync(signedIn);
        using (var uow = services.GetRequiredService<IUnitOfWorkManager>().Begin())
        {
            await Should.ThrowAsync<UserFriendlyException>(() => page.OnGetExternalLoginCallbackAsync());
            await uow.CompleteAsync();
        }

        var log = (await services.GetRequiredService<IIdentitySecurityLogRepository>()
            .GetListAsync(identity: IdentitySecurityLogIdentityConsts.IdentityExternal)).ShouldHaveSingleItem();
        log.Action.ShouldBe(IdentitySecurityLogActionConsts.LoginNotAllowed);
        log.UserId.ShouldBe(target.Id);
        log.UserName.ShouldBe(target.UserName);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("new-user@example.test")]
    public async Task Callback_Should_Redirect_Unregistered_User_Without_Recording_A_Login(string email)
    {
        using var scope = ServiceProvider.CreateScope();
        var (page, authentication) = CreatePage(scope.ServiceProvider, email);
        var redirect = (await page.OnGetExternalLoginCallbackAsync()).ShouldBeOfType<RedirectToPageResult>();
        redirect.PageName.ShouldBe("./Register");
        await authentication.DidNotReceive().SignInAsync(Arg.Any<HttpContext>(), IdentityConstants.ApplicationScheme,
            Arg.Any<ClaimsPrincipal>(), Arg.Any<AuthenticationProperties>());
        (await scope.ServiceProvider.GetRequiredService<IIdentitySecurityLogRepository>()
            .GetListAsync(identity: IdentitySecurityLogIdentityConsts.IdentityExternal)).ShouldBeEmpty();
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public async Task Callback_Should_Not_Record_A_Login_Without_An_External_Ticket(bool remoteError)
    {
        using var scope = ServiceProvider.CreateScope();
        var (page, authentication) = CreatePage(scope.ServiceProvider, null);
        authentication.AuthenticateAsync(page.HttpContext, IdentityConstants.ExternalScheme)
            .Returns(AuthenticateResult.NoResult());
        var redirect = (await page.OnGetExternalLoginCallbackAsync(remoteError: remoteError ? "access_denied" : null))
            .ShouldBeOfType<RedirectToPageResult>();
        redirect.PageName.ShouldBe("./Login");
        await authentication.DidNotReceive().SignInAsync(Arg.Any<HttpContext>(), IdentityConstants.ApplicationScheme,
            Arg.Any<ClaimsPrincipal>(), Arg.Any<AuthenticationProperties>());
        (await scope.ServiceProvider.GetRequiredService<IIdentitySecurityLogRepository>()
            .GetListAsync(identity: IdentitySecurityLogIdentityConsts.IdentityExternal)).ShouldBeEmpty();
    }

    protected static (LoginModel Page, IAuthenticationService Authentication) CreatePage(IServiceProvider services, string email)
    {
        var page = services.GetRequiredService<LoginModel>();
        var context = new DefaultHttpContext { RequestServices = services };
        services.GetRequiredService<IHttpContextAccessor>().HttpContext = context;
        page.PageContext = new PageContext { HttpContext = context };
        page.Url = Substitute.For<IUrlHelper>();
        page.Url.IsLocalUrl(Arg.Any<string>()).Returns(true);
        var principal = new ClaimsPrincipal(new ClaimsIdentity(new[] { new Claim(ClaimTypes.NameIdentifier, "key") }, "Example"));
        if (email != null)
        {
            ((ClaimsIdentity)principal.Identity).AddClaim(new Claim(ClaimTypes.Email, email));
        }
        var properties = new AuthenticationProperties();
        properties.Items["LoginProvider"] = "Example";
        var authentication = Substitute.For<IAuthenticationService>();
        authentication.AuthenticateAsync(context, IdentityConstants.ExternalScheme)
            .Returns(AuthenticateResult.Success(new AuthenticationTicket(principal, properties, IdentityConstants.ExternalScheme)));
        authentication.AuthenticateAsync(context, Arg.Is<string>(scheme => scheme != IdentityConstants.ExternalScheme))
            .Returns(AuthenticateResult.NoResult());
        context.RequestServices = new AuthenticationServiceProvider(services, authentication);
        return (page, authentication);
    }

    private sealed class AuthenticationServiceProvider(IServiceProvider services, IAuthenticationService authentication) : IServiceProvider
    {
        public object GetService(Type serviceType) => serviceType == typeof(IAuthenticationService)
            ? authentication
            : services.GetService(serviceType);
    }
}

public class ExternalLoginSecurityLog_SharedUser_Tests : ExternalLoginSecurityLog_Tests
{
    protected override void AfterAddApplication(IServiceCollection services)
    {
        base.AfterAddApplication(services);
        services.Configure<AbpMultiTenancyOptions>(options =>
        {
            options.IsEnabled = true;
            options.UserSharingStrategy = TenantUserSharingStrategy.Shared;
        });
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public async Task Callback_From_Another_Tenant_Should_Log_Under_The_Account_Tenant(bool active)
    {
        using var scope = ServiceProvider.CreateScope();
        var services = scope.ServiceProvider;
        var manager = services.GetRequiredService<IdentityUserManager>();
        var user = new IdentityUser(Guid.NewGuid(), "shared-user", "shared-user@example.test");
        using (var uow = services.GetRequiredService<IUnitOfWorkManager>().Begin())
        {
            (await manager.CreateAsync(user)).CheckErrors();
            (await manager.AddLoginAsync(user, new UserLoginInfo("Example", "key", "Example"))).CheckErrors();
            user.SetIsActive(active);
            (await manager.UpdateAsync(user)).CheckErrors();
            await uow.CompleteAsync();
        }

        var cache = services.GetRequiredService<IDistributedCache<AbpDynamicClaimCacheItem>>();
        var cacheKey = AbpDynamicClaimCacheItem.CalculateCacheKey(user.Id, user.TenantId);
        await cache.SetAsync(cacheKey, new AbpDynamicClaimCacheItem());

        var (page, _) = CreatePage(services, user.Email);
        using (services.GetRequiredService<ICurrentTenant>().Change(Guid.NewGuid()))
        using (var uow = services.GetRequiredService<IUnitOfWorkManager>().Begin())
        {
            if (active)
            {
                await page.OnGetExternalLoginCallbackAsync("/");
            }
            else
            {
                await Should.ThrowAsync<UserFriendlyException>(() => page.OnGetExternalLoginCallbackAsync());
            }
            await uow.CompleteAsync();
        }

        using (services.GetRequiredService<IDataFilter>().Disable<IMultiTenant>())
        {
            var log = (await services.GetRequiredService<IIdentitySecurityLogRepository>()
                .GetListAsync(identity: IdentitySecurityLogIdentityConsts.IdentityExternal)).ShouldHaveSingleItem();
            log.Action.ShouldBe(active ? IdentitySecurityLogActionConsts.LoginSucceeded : IdentitySecurityLogActionConsts.LoginNotAllowed);
            log.TenantId.ShouldBeNull();
            log.UserId.ShouldBe(user.Id);
            log.UserName.ShouldBe(user.UserName);
        }

        if (active)
        {
            (await cache.GetAsync(cacheKey)).ShouldBeNull();
        }
    }
}
