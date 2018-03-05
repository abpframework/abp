using Volo.Abp.AspNetCore.Authentication.OAuth.Claims;
using Volo.Abp.Security.Claims;

namespace Microsoft.AspNetCore.Authentication.OAuth.Claims
{
    public static class AbpClaimActionCollectionExtensions
    {
        public static void MapAbpClaimTypes(this ClaimActionCollection claimActions)
        {
            claimActions.MapJsonKey(AbpClaimTypes.Email, "email");
            claimActions.MapJsonKey(AbpClaimTypes.UserName, "name");
            claimActions.MapJsonKey(AbpClaimTypes.EmailVerified, "email_verified");
            claimActions.MapJsonKey(AbpClaimTypes.PhoneNumber, "phone_number");
            claimActions.MapJsonKey(AbpClaimTypes.PhoneNumberVerified, "phone_number_verified");

            claimActions.MapJsonKeyMultiple(AbpClaimTypes.Role, "role");

            claimActions.DeleteClaim("name");
            claimActions.DeleteClaim("email");
        }

        public static void MapJsonKeyMultiple(this ClaimActionCollection claimActions, string claimType, string jsonKey)
        {
            claimActions.Add(new MultipleClaimAction(claimType, jsonKey));
        }
    }
}
