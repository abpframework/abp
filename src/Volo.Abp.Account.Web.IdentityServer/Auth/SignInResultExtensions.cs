using Microsoft.AspNetCore.Identity;

namespace Volo.Abp.Account.Web.Auth
{
    //TODO: Move to the framework..?
    public static class SignInResultExtensions
    {
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
}
