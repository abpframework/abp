using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using Volo.CmsKit.EntityFrameworkCore;

namespace Volo.CmsKit.Tags
{
    public class EfCoreEntityTagRepository: EfCoreRepository<ICmsKitDbContext, EntityTag>, IEntityTagRepository
    {
        public EfCoreEntityTagRepository(IDbContextProvider<ICmsKitDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }
    }
}