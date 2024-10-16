using Volo.Abp.Users;

namespace Volo.CmsKit.Users;

public class CmsUserLookupService : UserLookupService<CmsUser, ICmsUserRepository>, ICmsUserLookupService
{
    public CmsUserLookupService()
    {
    }

    protected override CmsUser CreateUser(IUserData externalUser)
    {
        return new CmsUser(externalUser);
    }
}