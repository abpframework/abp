using System;
using Volo.Abp.Identity;

namespace Microsoft.AspNetCore.Identity;

public static class AbpIdentityResultExtensions
{
    public static void CheckErrors(this IdentityResult identityResult)
    {
        if (identityResult.Succeeded)
        {
            return;
        }

        if (identityResult.Errors == null)
        {
            throw new ArgumentException("identityResult.Errors should not be null.");
        }

        throw new AbpIdentityResultException(identityResult);
    }

    public static string GetResultAsString(this SignInResult signInResult)
    {
        if (signInResult.Succeeded)
        {
            return "Succeeded";
        }

        if (signInResult.IsLockedOut)
        {
            return "IsLockedOut";
        }

        if (signInResult.IsNotAllowed)
        {
            return "IsNotAllowed";
        }

        if (signInResult.RequiresTwoFactor)
        {
            return "RequiresTwoFactor";
        }

        return "Unknown";
    }
}
