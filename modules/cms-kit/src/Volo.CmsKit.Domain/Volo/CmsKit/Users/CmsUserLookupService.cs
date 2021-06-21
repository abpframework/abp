using Volo.Abp.Uow;
using Volo.Abp.Users;

namespace Volo.CmsKit.Users
{
    public class CmsUserLookupService: UserLookupService<CmsUser, ICmsUserRepository>, ICmsUserLookupService
    {
        public CmsUserLookupService(
            ICmsUserRepository userRepository,
            IUnitOfWorkManager unitOfWorkManager)
            : base(
                userRepository,
                unitOfWorkManager)
        {

        }

        protected override CmsUser CreateUser(IUserData externalUser)
        {
            return new CmsUser(externalUser);
        }
    }
}
