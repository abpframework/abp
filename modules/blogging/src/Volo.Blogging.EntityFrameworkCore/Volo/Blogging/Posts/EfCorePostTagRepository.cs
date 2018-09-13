using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using Volo.Blogging.EntityFrameworkCore;

namespace Volo.Blogging.Posts
{
    public class EfCorePostTagRepository : EfCoreRepository<IBloggingDbContext, PostTag>, IPostTagRepository
    {
        public EfCorePostTagRepository(IDbContextProvider<IBloggingDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }

        public void DeleteOfPost(Guid id)
        {
            var recordsToDelete = DbSet.Where(pt=>pt.PostId == id);
            DbSet.RemoveRange(recordsToDelete);
        }

        public async Task<PostTag> FindByTagIdAndPostIdAsync(Guid postId, Guid tagId)
        {
            return await DbSet.FirstOrDefaultAsync(pt=> pt.PostId == postId && pt.TagId == tagId);
        }
    }
}
