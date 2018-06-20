using Volo.Abp.Users;

namespace Volo.Abp.Identity
{
    public static class IdentityUserDtoExtensions
    {
        public static IUserData ToUserInfo(this IdentityUserDto user)
        {
            return new UserData(
                user.Id,
                user.UserName,
                user.Email,
                user.EmailConfirmed,
                user.PhoneNumber,
                user.PhoneNumberConfirmed,
                user.TenantId
            );
        }
    }
}
