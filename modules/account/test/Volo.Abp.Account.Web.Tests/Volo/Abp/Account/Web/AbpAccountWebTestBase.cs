using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NSubstitute;
using Volo.Abp.Account.Localization;
using Volo.Abp.Account.Web.Pages.Account;
using Volo.Abp.AspNetCore.Mvc.UI.Alerts;
using Volo.Abp.Identity;
using Volo.Abp.Identity.Settings;
using Volo.Abp.Modularity;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Settings;
using Volo.Abp.Testing;
using Volo.Abp.Uow;
using IdentityRole = Volo.Abp.Identity.IdentityRole;
using IdentityUser = Volo.Abp.Identity.IdentityUser;

namespace Volo.Abp.Account.Web;

public enum RestrictedUserState
{
    Inactive,
    LockedOut,
    RequiresConfirmation,
    MustChangePassword,
    PasswordExpired
}

public abstract class AbpAccountWebTestBase : AbpAccountWebTestBase<AbpAccountWebTestModule>
{
}

public abstract class AbpAccountWebTestBase<TStartupModule> : AbpIntegratedTest<TStartupModule>
    where TStartupModule : IAbpModule
{
    protected const string Password = "1q2w3E*";
    protected const string ExternalLoginProvider = "TestProvider";

    protected override void SetAbpApplicationCreationOptions(AbpApplicationCreationOptions options)
    {
        options.UseAutofac();
    }

    // The sign-in manager of an application, replaced like it is documented for ASP.NET Core Identity.
    protected static void ReplaceSignInManager(IServiceCollection services)
    {
        new IdentityBuilder(typeof(IdentityUser), typeof(IdentityRole), services).AddSignInManager<TestSignInManager>();
    }

    protected void SetSetting(string name, string value)
    {
        ((TestSettingStore)GetRequiredService<ISettingStore>()).GlobalValues[name] = value;
    }

    protected async Task<IdentityUser> CreateUserAsync(Guid? tenantId = null, params RestrictedUserState[] states)
    {
        if (states.Contains(RestrictedUserState.RequiresConfirmation))
        {
            SetSetting(IdentitySettingNames.SignIn.RequireConfirmedEmail, "true");
        }

        if (states.Contains(RestrictedUserState.PasswordExpired))
        {
            SetSetting(IdentitySettingNames.Password.ForceUsersToPeriodicallyChangePassword, "true");
            SetSetting(IdentitySettingNames.Password.PasswordChangePeriodDays, "1");
        }

        using (GetRequiredService<ICurrentTenant>().Change(tenantId))
        using (var uow = GetRequiredService<IUnitOfWorkManager>().Begin())
        {
            var userManager = GetRequiredService<IdentityUserManager>();
            var userName = "user-" + Guid.NewGuid().ToString("N");
            var user = new IdentityUser(Guid.NewGuid(), userName, userName + "@abp.io", tenantId);
            user.SetEmailConfirmed(!states.Contains(RestrictedUserState.RequiresConfirmation));
            (await userManager.CreateAsync(user, Password)).CheckErrors();

            foreach (var state in states)
            {
                switch (state)
                {
                    case RestrictedUserState.Inactive:
                        user.SetIsActive(false);
                        break;
                    case RestrictedUserState.LockedOut:
                        (await userManager.SetLockoutEnabledAsync(user, true)).CheckErrors();
                        (await userManager.SetLockoutEndDateAsync(user, DateTimeOffset.UtcNow.AddHours(1))).CheckErrors();
                        break;
                    case RestrictedUserState.MustChangePassword:
                        user.SetShouldChangePasswordOnNextLogin(true);
                        break;
                    case RestrictedUserState.PasswordExpired:
                        user.SetLastPasswordChangeTime(DateTimeOffset.UtcNow.AddDays(-30));
                        break;
                }
            }

            (await userManager.UpdateAsync(user)).CheckErrors();
            await uow.CompleteAsync();
            return user;
        }
    }

    /// <summary>
    /// Creates the page like it is created for a request.
    /// </summary>
    /// <param name="serviceProvider">The services of the request</param>
    /// <param name="externalLoginPrincipal">The principal of the external login, if the request is a callback of it</param>
    protected TPage CreatePage<TPage>(IServiceProvider serviceProvider, ClaimsPrincipal externalLoginPrincipal = null)
        where TPage : AccountPageModel
    {
        var httpContext = new DefaultHttpContext { RequestServices = serviceProvider };
        serviceProvider.GetRequiredService<IHttpContextAccessor>().HttpContext = httpContext;

        var authenticationService = serviceProvider.GetRequiredService<IAuthenticationService>();
        authenticationService.AuthenticateAsync(Arg.Any<HttpContext>(), Arg.Any<string>()).Returns(AuthenticateResult.NoResult());
        if (externalLoginPrincipal != null)
        {
            var properties = new AuthenticationProperties();
            properties.Items["LoginProvider"] = ExternalLoginProvider;
            authenticationService.AuthenticateAsync(Arg.Any<HttpContext>(), IdentityConstants.ExternalScheme).Returns(
                AuthenticateResult.Success(new AuthenticationTicket(externalLoginPrincipal, properties, IdentityConstants.ExternalScheme)));
        }

        var page = serviceProvider.GetRequiredService<TPage>();
        var modelState = new ModelStateDictionary();
        var actionContext = new ActionContext(httpContext, new RouteData(), new PageActionDescriptor(), modelState);
        page.PageContext = new PageContext(actionContext)
        {
            ViewData = new ViewDataDictionary(new EmptyModelMetadataProvider(), modelState)
        };
        page.TempData = new TempDataDictionary(httpContext, Substitute.For<ITempDataProvider>());
        page.Url = new UrlHelper(actionContext);
        return page;
    }

    protected bool IsSignedIn()
    {
        return GetRequiredService<IAuthenticationService>().ReceivedCalls().Any(call =>
            call.GetMethodInfo().Name == nameof(IAuthenticationService.SignInAsync) &&
            (string)call.GetArguments()[1] == IdentityConstants.ApplicationScheme);
    }

    protected static string[] GetAlerts(IServiceProvider serviceProvider)
    {
        return serviceProvider.GetRequiredService<IAlertManager>().Alerts.Select(x => x.Text).ToArray();
    }

    protected string L(string name)
    {
        return GetRequiredService<IStringLocalizer<AccountResource>>()[name];
    }
}

public class TestSignInManager : SignInManager<IdentityUser>
{
    public TestSignInManager(
        UserManager<IdentityUser> userManager,
        IHttpContextAccessor contextAccessor,
        IUserClaimsPrincipalFactory<IdentityUser> claimsFactory,
        IOptions<IdentityOptions> optionsAccessor,
        ILogger<SignInManager<IdentityUser>> logger,
        IAuthenticationSchemeProvider schemes,
        IUserConfirmation<IdentityUser> confirmation)
        : base(userManager, contextAccessor, claimsFactory, optionsAccessor, logger, schemes, confirmation)
    {
    }
}
