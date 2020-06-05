using Volo.Abp.AspNetCore.Authentication.OAuth.Claims;
using Volo.Abp.Security.Claims;

namespace Microsoft.AspNetCore.Authentication.OAuth.Claims
{
    public static class AbpClaimActionCollectionExtensions
    {
        public static void MapAbpClaimTypes(this ClaimActionCollection claimActions)
        {
            if (AbpClaimTypes.UserName != "name")
            {
                claimActions.MapJsonKey(AbpClaimTypes.UserName, "name");
                claimActions.DeleteClaim("name");
            }

            if (AbpClaimTypes.Email != "email")
            {
                claimActions.MapJsonKey(AbpClaimTypes.Email, "email");
                claimActions.DeleteClaim("email");
            }

            if (AbpClaimTypes.EmailVerified != "email_verified")
            {
                claimActions.MapJsonKey(AbpClaimTypes.EmailVerified, "email_verified");
            }

            if (AbpClaimTypes.PhoneNumber != "phone_number")
            {
                claimActions.MapJsonKey(AbpClaimTypes.PhoneNumber, "phone_number");
            }

            if (AbpClaimTypes.PhoneNumberVerified != "phone_number_verified")
            {
                claimActions.MapJsonKey(AbpClaimTypes.PhoneNumberVerified, "phone_number_verified");
            }

            if (AbpClaimTypes.Role != "role")
            {
                claimActions.MapJsonKeyMultiple(AbpClaimTypes.Role, "role");
            }
        }

        public static void MapJsonKeyMultiple(this ClaimActionCollection claimActions, string claimType, string jsonKey)
        {
            claimActions.Add(new MultipleClaimAction(claimType, jsonKey));
        }
    }
}
