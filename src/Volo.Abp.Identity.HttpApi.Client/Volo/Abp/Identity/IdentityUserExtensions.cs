using Volo.Abp.Users;

namespace Volo.Abp.Identity
{
    public static class IdentityUserDtoExtensions
    {
        public static IUserInfo ToUserInfo(this IdentityUserDto user)
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
