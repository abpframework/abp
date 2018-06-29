using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using Volo.Blogging.Blogs;
using Volo.Blogging.EntityFrameworkCore;

namespace Volo.Blogging.Posts
{
    public class EfCorePostRepository : EfCoreRepository<IBloggingDbContext, Post, Guid>, IPostRepository
    {
        public EfCorePostRepository(IDbContextProvider<IBloggingDbContext> dbContextProvider)
            : base(dbContextProvider)
        {

        }

        public List<Post> GetPostsByBlogId(Guid id)
        {
            return DbSet.Where(p => p.BlogId == id).ToList();
        }

        public async Task<Post> GetPostByUrl(Guid blogId, string url)
        {
            return await DbSet.FirstOrDefaultAsync(p => p.BlogId == blogId && p.Url == url);
        }
    }
}
