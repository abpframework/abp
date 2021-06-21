using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Users.EntityFrameworkCore;
using Volo.CmsKit.EntityFrameworkCore;

namespace Volo.CmsKit.Users
{
    public class EfCoreCmsUserRepository: EfCoreUserRepositoryBase<ICmsKitDbContext, CmsUser>, ICmsUserRepository
    {
        public EfCoreCmsUserRepository(IDbContextProvider<ICmsKitDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }
    }
}
