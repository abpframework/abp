using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using Volo.CmsKit.EntityFrameworkCore;
using Volo.CmsKit.Tags;

namespace Volo.CmsKit.EntityTags
{
    public class EfCoreEntityTagRepository : EfCoreRepository<ICmsKitDbContext, EntityTag>, IEntityTagRepository
    {
        public EfCoreEntityTagRepository(IDbContextProvider<ICmsKitDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }
    }
}
