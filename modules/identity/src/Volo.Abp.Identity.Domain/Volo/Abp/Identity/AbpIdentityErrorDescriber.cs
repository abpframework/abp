using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Identity.Localization;

namespace Volo.Abp.Identity;

[Dependency(ServiceLifetime.Scoped, ReplaceServices = true)]
[ExposeServices(typeof(IdentityErrorDescriber))]
public class AbpIdentityErrorDescriber : IdentityErrorDescriber
{
    protected IStringLocalizer<IdentityResource> Localizer { get; }

    public AbpIdentityErrorDescriber(IStringLocalizer<IdentityResource> localizer)
    {
        Localizer = localizer;
    }

    public override IdentityError DefaultError()
    {
        return new IdentityError
        {
            Code = nameof(DefaultError),
            Description = Localizer["Volo.Abp.Identity:DefaultError"]
        };
    }

    public override IdentityError ConcurrencyFailure()
    {
        return new IdentityError
        {
            Code = nameof(ConcurrencyFailure),
            Description = Localizer["Volo.Abp.Identity:ConcurrencyFailure"]
        };
    }

    public override IdentityError PasswordMismatch()
    {
        return new IdentityError
        {
            Code = nameof(PasswordMismatch),
            Description = Localizer["Volo.Abp.Identity:PasswordMismatch"]
        };
    }

    public override IdentityError InvalidToken()
    {
        return new IdentityError
        {
            Code = nameof(InvalidToken),
            Description = Localizer["Volo.Abp.Identity:InvalidToken"]
        };
    }

    public override IdentityError RecoveryCodeRedemptionFailed()
    {
        return new IdentityError
        {
            Code = nameof(RecoveryCodeRedemptionFailed),
            Description = Localizer["Volo.Abp.Identity:RecoveryCodeRedemptionFailed"]
        };
    }

    public override IdentityError LoginAlreadyAssociated()
    {
        return new IdentityError
        {
            Code = nameof(LoginAlreadyAssociated),
            Description = Localizer["Volo.Abp.Identity:LoginAlreadyAssociated"]
        };
    }

    public override IdentityError InvalidUserName(string? userName)
    {
        return new IdentityError
        {
            Code = nameof(InvalidUserName),
            Description = Localizer["Volo.Abp.Identity:InvalidUserName", userName ?? ""]
        };
    }

    public override IdentityError InvalidEmail(string? email)
    {
        return new IdentityError
        {
            Code = nameof(InvalidEmail),
            Description = Localizer["Volo.Abp.Identity:InvalidEmail", email ?? ""]
        };
    }

    public override IdentityError DuplicateUserName(string userName)
    {
        return new IdentityError
        {
            Code = nameof(DuplicateUserName),
            Description = Localizer["Volo.Abp.Identity:DuplicateUserName", userName]
        };
    }

    public override IdentityError DuplicateEmail(string email)
    {
        return new IdentityError
        {
            Code = nameof(DuplicateEmail),
            Description = Localizer["Volo.Abp.Identity:DuplicateEmail", email]
        };
    }

    public override IdentityError InvalidRoleName(string? role)
    {
        return new IdentityError
        {
            Code = nameof(InvalidRoleName),
            Description = Localizer["Volo.Abp.Identity:InvalidRoleName", role ?? ""]
        };
    }

    public override IdentityError DuplicateRoleName(string role)
    {
        return new IdentityError
        {
            Code = nameof(DuplicateRoleName),
            Description = Localizer["Volo.Abp.Identity:DuplicateRoleName", role]
        };
    }

    public override IdentityError UserAlreadyHasPassword()
    {
        return new IdentityError
        {
            Code = nameof(UserAlreadyHasPassword),
            Description = Localizer["Volo.Abp.Identity:UserAlreadyHasPassword"]
        };
    }

    public override IdentityError UserLockoutNotEnabled()
    {
        return new IdentityError
        {
            Code = nameof(UserLockoutNotEnabled),
            Description = Localizer["Volo.Abp.Identity:UserLockoutNotEnabled"]
        };
    }

    public override IdentityError UserAlreadyInRole(string role)
    {
        return new IdentityError
        {
            Code = nameof(UserAlreadyInRole),
            Description = Localizer["Volo.Abp.Identity:UserAlreadyInRole", role]
        };
    }

    public override IdentityError UserNotInRole(string role)
    {
        return new IdentityError
        {
            Code = nameof(UserNotInRole),
            Description = Localizer["Volo.Abp.Identity:UserNotInRole", role]
        };
    }

    public override IdentityError PasswordTooShort(int length)
    {
        return new IdentityError
        {
            Code = nameof(PasswordTooShort),
            Description = Localizer["Volo.Abp.Identity:PasswordTooShort", length]
        };
    }

    public override IdentityError PasswordRequiresUniqueChars(int uniqueChars)
    {
        return new IdentityError
        {
            Code = nameof(PasswordRequiresUniqueChars),
            Description = Localizer["Volo.Abp.Identity:PasswordRequiresUniqueChars", uniqueChars]
        };
    }

    public override IdentityError PasswordRequiresNonAlphanumeric()
    {
        return new IdentityError
        {
            Code = nameof(PasswordRequiresNonAlphanumeric),
            Description = Localizer["Volo.Abp.Identity:PasswordRequiresNonAlphanumeric"]
        };
    }


    public override IdentityError PasswordRequiresDigit()
    {
        return new IdentityError
        {
            Code = nameof(PasswordRequiresDigit),
            Description = Localizer["Volo.Abp.Identity:PasswordRequiresDigit"]
        };
    }

    public override IdentityError PasswordRequiresLower()
    {
        return new IdentityError
        {
            Code = nameof(PasswordRequiresLower),
            Description = Localizer["Volo.Abp.Identity:PasswordRequiresLower"]
        };
    }

    public override IdentityError PasswordRequiresUpper()
    {
        return new IdentityError
        {
            Code = nameof(PasswordRequiresUpper),
            Description = Localizer["Volo.Abp.Identity:PasswordRequiresUpper"]
        };
    }
}
