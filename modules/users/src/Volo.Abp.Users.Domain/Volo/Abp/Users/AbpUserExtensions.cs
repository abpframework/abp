namespace Volo.Abp.Users
{
    public static class AbpUserExtensions
    {
        public static IUserData ToAbpUserData(this IUser user)
        {
            return new UserData(
                user.Id,
                user.UserName,
                user.Name,
                user.Surname,
                user.Email,
                user.EmailConfirmed,
                user.PhoneNumber,
                user.PhoneNumberConfirmed,
                user.TenantId
            );
        }
    }
}