using System;
using System.Collections.Specialized;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer4.Models;
using IdentityServer4.Validation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Volo.Abp.Identity;
using Volo.Abp.Uow;
using IdentityUser = Volo.Abp.Identity.IdentityUser;

namespace Volo.Abp.IdentityServer.AspNetIdentity;

public abstract class AbpResourceOwnerPasswordValidatorTestBase : AbpIdentityServerDomainTestBase
{
    protected const string UserName = "password-grant-user";
    protected const string Password = "1q2w3E*";
    protected const string NewPassword = "2q3w4E*";

    protected Task CreateUserAsync(
        bool twoFactorEnabled,
        bool shouldChangePasswordOnNextLogin = false,
        bool addFailedAccess = false,
        DateTimeOffset? lastPasswordChangeTime = null)
    {
        return WithUnitOfWorkAsync(async serviceProvider =>
        {
            var userManager = serviceProvider.GetRequiredService<IdentityUserManager>();
            var user = new IdentityUser(Guid.NewGuid(), UserName, UserName + "@abp.io");
            user.SetEmailConfirmed(true);
            user.SetShouldChangePasswordOnNextLogin(shouldChangePasswordOnNextLogin);

            (await userManager.CreateAsync(user, Password)).CheckErrors();
            (await userManager.SetLockoutEnabledAsync(user, true)).CheckErrors();
            (await userManager.SetTwoFactorEnabledAsync(user, twoFactorEnabled)).CheckErrors();

            if (lastPasswordChangeTime.HasValue)
            {
                user.SetLastPasswordChangeTime(lastPasswordChangeTime);
                (await userManager.UpdateAsync(user)).CheckErrors();
            }

            if (addFailedAccess)
            {
                (await userManager.AccessFailedAsync(user)).CheckErrors();
            }
        });
    }

    protected Task<string> GenerateTwoFactorCodeAsync()
    {
        return WithUnitOfWorkAsync(async serviceProvider =>
        {
            var userManager = serviceProvider.GetRequiredService<IdentityUserManager>();
            var user = await userManager.FindByNameAsync(UserName);
            return await userManager.GenerateTwoFactorTokenAsync(user, TokenOptions.DefaultEmailProvider);
        });
    }

    protected Task<string> GenerateRecoveryCodeAsync()
    {
        return WithUnitOfWorkAsync(async serviceProvider =>
        {
            var userManager = serviceProvider.GetRequiredService<IdentityUserManager>();
            var user = await userManager.FindByNameAsync(UserName);
            var recoveryCodes = await userManager.GenerateNewTwoFactorRecoveryCodesAsync(user, 1);
            return recoveryCodes.Single();
        });
    }

    protected Task<int> GetAccessFailedCountAsync()
    {
        return WithUnitOfWorkAsync(async serviceProvider =>
        {
            var userManager = serviceProvider.GetRequiredService<IdentityUserManager>();
            var user = await userManager.FindByNameAsync(UserName);
            return await userManager.GetAccessFailedCountAsync(user);
        });
    }

    protected Task<(bool OldPasswordIsValid, bool NewPasswordIsValid, bool ShouldChangePasswordOnNextLogin)> GetPasswordStateAsync()
    {
        return WithUnitOfWorkAsync(async serviceProvider =>
        {
            var userManager = serviceProvider.GetRequiredService<IdentityUserManager>();
            var user = await userManager.FindByNameAsync(UserName);
            return (
                OldPasswordIsValid: await userManager.CheckPasswordAsync(user, Password),
                NewPasswordIsValid: await userManager.CheckPasswordAsync(user, NewPassword),
                user.ShouldChangePasswordOnNextLogin);
        });
    }

    protected Task ValidateAsync(ResourceOwnerPasswordValidationContext context)
    {
        return WithUnitOfWorkAsync(
            new AbpUnitOfWorkOptions { IsTransactional = true },
            async serviceProvider =>
            {
                var unitOfWorkManager = serviceProvider.GetRequiredService<IUnitOfWorkManager>();
                unitOfWorkManager.Current.ShouldNotBeNull();
                unitOfWorkManager.Current.Options.IsTransactional.ShouldBeTrue();

                var httpContextAccessor = serviceProvider.GetRequiredService<IHttpContextAccessor>();
                var originalHttpContext = httpContextAccessor.HttpContext;
                httpContextAccessor.HttpContext = new DefaultHttpContext
                {
                    RequestServices = serviceProvider
                };
                try
                {
                    await serviceProvider
                        .GetRequiredService<IResourceOwnerPasswordValidator>()
                        .ValidateAsync(context);
                }
                finally
                {
                    httpContextAccessor.HttpContext = originalHttpContext;
                }
            });
    }

    protected static ResourceOwnerPasswordValidationContext CreateContext(
        NameValueCollection raw = null,
        string password = null)
    {
        return new ResourceOwnerPasswordValidationContext
        {
            UserName = UserName,
            Password = password ?? Password,
            Request = new ValidatedTokenRequest
            {
                Raw = raw ?? new NameValueCollection(),
                Client = new Client { ClientId = "test-client" }
            }
        };
    }

    protected Task WithUnitOfWorkAsync(Func<IServiceProvider, Task> action)
    {
        return WithUnitOfWorkAsync(new AbpUnitOfWorkOptions(), action);
    }

    protected async Task WithUnitOfWorkAsync(
        AbpUnitOfWorkOptions options,
        Func<IServiceProvider, Task> action)
    {
        using var scope = ServiceProvider.CreateScope();
        var unitOfWorkManager = scope.ServiceProvider.GetRequiredService<IUnitOfWorkManager>();
        using var uow = unitOfWorkManager.Begin(options);
        await action(scope.ServiceProvider);
        await uow.CompleteAsync();
    }

    protected Task<TResult> WithUnitOfWorkAsync<TResult>(Func<IServiceProvider, Task<TResult>> action)
    {
        return WithUnitOfWorkAsync(new AbpUnitOfWorkOptions(), action);
    }

    protected async Task<TResult> WithUnitOfWorkAsync<TResult>(
        AbpUnitOfWorkOptions options,
        Func<IServiceProvider, Task<TResult>> action)
    {
        using var scope = ServiceProvider.CreateScope();
        var unitOfWorkManager = scope.ServiceProvider.GetRequiredService<IUnitOfWorkManager>();
        using var uow = unitOfWorkManager.Begin(options);
        var result = await action(scope.ServiceProvider);
        await uow.CompleteAsync();
        return result;
    }
}
