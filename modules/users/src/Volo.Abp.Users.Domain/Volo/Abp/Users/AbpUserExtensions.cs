namespace Volo.Abp.Users;

public static class AbpUserExtensions
{
    public static IUserData ToAbpUserData(this IUser user)
    {
        return new UserData(
            id: user.Id,
            userName: user.UserName,
            email: user.Email,
            name: user.Name,
            surname: user.Surname,
            isActive: user.IsActive,
            emailConfirmed: user.EmailConfirmed,
            phoneNumber: user.PhoneNumber,
            phoneNumberConfirmed: user.PhoneNumberConfirmed,
            tenantId: user.TenantId,
            entityVersion: user.EntityVersion,
            extraProperties: user.ExtraProperties
        );
    }

    public static UserEto ToUserEto(this IUserData user)
    {
        return new UserEto(
            id: user.Id,
            userName: user.UserName,
            email: user.Email,
            name: user.Name,
            surname: user.Surname,
            isActive: user.IsActive,
            emailConfirmed: user.EmailConfirmed,
            phoneNumber: user.PhoneNumber,
            phoneNumberConfirmed: user.PhoneNumberConfirmed,
            tenantId: user.TenantId,
            entityVersion: user.EntityVersion,
            extraProperties: user.ExtraProperties
        );
    }
}