using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Shouldly;
using Volo.Abp.Identity.Settings;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Settings;
using Volo.Abp.Uow;
using Xunit;

namespace Volo.Abp.Identity.AspNetCore;

public class AbpSignInManager_PreSignInCheckWithoutPassword_Tests : AbpIdentityAspNetCoreTestBase
{
    [Theory]
    [InlineData(TenantUserSharingStrategy.Isolated, false)]
    [InlineData(TenantUserSharingStrategy.Shared, true)]
    public async Task Should_Only_Check_Password_Change_Period_If_Password_Is_Not_Shared(TenantUserSharingStrategy strategy, bool isAllowed)
    {
        var result = await CheckUserWithExpiredPasswordAsync(strategy);

        if (isAllowed)
        {
            result.ShouldBeNull();
        }
        else
        {
            result.ShouldBe(SignInResult.NotAllowed);
        }
    }

    [Fact]
    public async Task Should_Not_Allow_User_That_Should_Change_Password_On_Next_Login()
    {
        var result = await CheckUserWithExpiredPasswordAsync(TenantUserSharingStrategy.Shared, (_, user) =>
        {
            user.SetShouldChangePasswordOnNextLogin(true);
            return Task.CompletedTask;
        });

        result.ShouldBe(SignInResult.NotAllowed);
    }

    [Fact]
    public async Task Should_Not_Allow_Locked_Out_User()
    {
        var result = await CheckUserWithExpiredPasswordAsync(TenantUserSharingStrategy.Shared, async (userManager, user) =>
        {
            (await userManager.SetLockoutEnabledAsync(user, true)).CheckErrors();
            (await userManager.SetLockoutEndDateAsync(user, DateTimeOffset.UtcNow.AddHours(1))).CheckErrors();
        });

        result.ShouldBe(SignInResult.LockedOut);
    }

    // Only the password change period is skipped, the other checks of a customized sign-in manager still run.
    [Fact]
    public async Task Should_Run_Custom_Pre_Sign_In_Check()
    {
        var result = await CheckUserWithExpiredPasswordAsync(
            TenantUserSharingStrategy.Shared,
            signInManager: ActivatorUtilities.CreateInstance<RejectingSignInManager>(ServiceProvider));

        result.ShouldBe(SignInResult.NotAllowed);
    }

    private async Task<SignInResult> CheckUserWithExpiredPasswordAsync(
        TenantUserSharingStrategy strategy,
        Func<IdentityUserManager, IdentityUser, Task> configureUser = null,
        AbpSignInManager signInManager = null)
    {
        GetRequiredService<IOptions<AbpMultiTenancyOptions>>().Value.UserSharingStrategy = strategy;
        TestSettingValueProvider.AddSetting(IdentitySettingNames.Password.ForceUsersToPeriodicallyChangePassword, true.ToString());
        TestSettingValueProvider.AddSetting(IdentitySettingNames.Password.PasswordChangePeriodDays, 1.ToString());

        try
        {
            using (var uow = GetRequiredService<IUnitOfWorkManager>().Begin())
            {
                var userManager = GetRequiredService<IdentityUserManager>();
                var userName = "expired-" + Guid.NewGuid().ToString("N");
                var user = new IdentityUser(Guid.NewGuid(), userName, userName + "@abp.io");
                (await userManager.CreateAsync(user, "1q2w3E*")).CheckErrors();
                user.SetLastPasswordChangeTime(DateTimeOffset.UtcNow.AddDays(-2));
                if (configureUser != null)
                {
                    await configureUser(userManager, user);
                }
                (await userManager.UpdateAsync(user)).CheckErrors();

                var result = await (signInManager ?? GetRequiredService<AbpSignInManager>()).CallPreSignInCheckWithoutPasswordAsync(user);
                await uow.CompleteAsync();
                return result;
            }
        }
        finally
        {
            TestSettingValueProvider.AddSetting(IdentitySettingNames.Password.ForceUsersToPeriodicallyChangePassword, false.ToString());
        }
    }
}

// A sign-in manager with a custom rule that rejects the user after the checks of the base class.
public class RejectingSignInManager : AbpSignInManager
{
    public RejectingSignInManager(
        IdentityUserManager userManager,
        IHttpContextAccessor contextAccessor,
        IUserClaimsPrincipalFactory<IdentityUser> claimsFactory,
        IOptions<IdentityOptions> optionsAccessor,
        ILogger<SignInManager<IdentityUser>> logger,
        IAuthenticationSchemeProvider schemes,
        IUserConfirmation<IdentityUser> confirmation,
        IOptions<AbpIdentityOptions> options,
        ISettingProvider settingProvider,
        ICurrentTenant currentTenant)
        : base(userManager, contextAccessor, claimsFactory, optionsAccessor, logger, schemes, confirmation, options, settingProvider, currentTenant)
    {
    }

    protected override async Task<SignInResult> PreSignInCheck(IdentityUser user)
    {
        return await base.PreSignInCheck(user) ?? SignInResult.NotAllowed;
    }
}
