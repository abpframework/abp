using Volo.Abp.Users;

namespace Volo.Abp.Identity
{
    public static class IdentityUserExtensions
    {
        public static IUserInfo ToUserInfo(this IdentityUser user)
        {
            return new UserInfo(
                user.Id,
                user.UserName,
                user.Email,
                user.EmailConfirmed,
                user.PhoneNumber,
                user.PhoneNumberConfirmed
            );
        }
    }
}
