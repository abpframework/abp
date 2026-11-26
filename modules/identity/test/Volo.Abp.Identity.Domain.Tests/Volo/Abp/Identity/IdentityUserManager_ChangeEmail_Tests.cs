using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Volo.Abp.Uow;
using Volo.Abp.Validation;
using Xunit;

namespace Volo.Abp.Identity;

public class IdentityUserManager_ChangeEmail_Tests : AbpIdentityDomainTestBase
{
    private const string EmailRejectedWithException = "rejected-with-exception@abp.io";

    private readonly IdentityUserManager _identityUserManager;
    private readonly IUnitOfWorkManager _unitOfWorkManager;

    public IdentityUserManager_ChangeEmail_Tests()
    {
        _identityUserManager = GetRequiredService<IdentityUserManager>();
        _unitOfWorkManager = GetRequiredService<IUnitOfWorkManager>();
    }

    protected override void AfterAddApplication(IServiceCollection services)
    {
        services.AddScoped<IUserValidator<IdentityUser>, ExceptionUserValidator>();
    }

    [Fact]
    public async Task Should_Change_Email()
    {
        var userId = await CreateUserAsync();

        await ChangeEmailAsync(userId, "change-email-new@abp.io", async (user, token) =>
        {
            (await _identityUserManager.ChangeEmailAsync(user, "change-email-new@abp.io", token)).Succeeded.ShouldBeTrue();
        });

        var changed = await GetUserAsync(userId);
        changed.Email.ShouldBe("change-email-new@abp.io");
        changed.NormalizedEmail.ShouldBe("CHANGE-EMAIL-NEW@ABP.IO");
        changed.EmailConfirmed.ShouldBeTrue();
    }

    [Fact]
    public async Task Should_Restore_The_User_When_The_Validation_Fails()
    {
        var userId = await CreateUserAsync();
        var before = await GetUserAsync(userId);

        await ChangeEmailAsync(userId, "not-an-email", async (user, token) =>
        {
            (await _identityUserManager.ChangeEmailAsync(user, "not-an-email", token)).Succeeded.ShouldBeFalse();
            ShouldBeUnchanged(user, before);
        });

        ShouldBeUnchanged(await GetUserAsync(userId), before);
    }

    [Fact]
    public async Task Should_Restore_The_User_When_A_Validator_Throws()
    {
        var userId = await CreateUserAsync();
        var before = await GetUserAsync(userId);

        await ChangeEmailAsync(userId, EmailRejectedWithException, async (user, token) =>
        {
            await Should.ThrowAsync<AbpValidationException>(() => _identityUserManager.ChangeEmailAsync(user, EmailRejectedWithException, token));
            ShouldBeUnchanged(user, before);
        });

        ShouldBeUnchanged(await GetUserAsync(userId), before);
    }

    private async Task<Guid> CreateUserAsync()
    {
        var user = new IdentityUser(Guid.NewGuid(), "change-email-" + Guid.NewGuid().ToString("N"), Guid.NewGuid().ToString("N") + "@abp.io");
        using (var uow = _unitOfWorkManager.Begin())
        {
            (await _identityUserManager.CreateAsync(user)).Succeeded.ShouldBeTrue();
            await uow.CompleteAsync();
        }

        return user.Id;
    }

    // The unit of work is completed even when the change fails, like a page that shows the error and returns normally.
    private async Task ChangeEmailAsync(Guid userId, string newEmail, Func<IdentityUser, string, Task> action)
    {
        string token;
        using (var uow = _unitOfWorkManager.Begin())
        {
            token = await _identityUserManager.GenerateChangeEmailTokenAsync(await _identityUserManager.GetByIdAsync(userId), newEmail);
            await uow.CompleteAsync();
        }

        using (var uow = _unitOfWorkManager.Begin())
        {
            await action(await _identityUserManager.GetByIdAsync(userId), token);
            await uow.CompleteAsync();
        }
    }

    private async Task<IdentityUser> GetUserAsync(Guid userId)
    {
        using (var uow = _unitOfWorkManager.Begin(requiresNew: true))
        {
            var user = await _identityUserManager.GetByIdAsync(userId);
            await uow.CompleteAsync();
            return user;
        }
    }

    private static void ShouldBeUnchanged(IdentityUser user, IdentityUser before)
    {
        user.Email.ShouldBe(before.Email);
        user.NormalizedEmail.ShouldBe(before.NormalizedEmail);
        user.EmailConfirmed.ShouldBe(before.EmailConfirmed);
        user.SecurityStamp.ShouldBe(before.SecurityStamp);
    }

    private class ExceptionUserValidator : IUserValidator<IdentityUser>
    {
        public Task<IdentityResult> ValidateAsync(UserManager<IdentityUser> manager, IdentityUser user)
        {
            if (user.Email == EmailRejectedWithException)
            {
                throw new AbpValidationException(new List<ValidationResult> { new("The email is not allowed.") });
            }

            return Task.FromResult(IdentityResult.Success);
        }
    }
}
