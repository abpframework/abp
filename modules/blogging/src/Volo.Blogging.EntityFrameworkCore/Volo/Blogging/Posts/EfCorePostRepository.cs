using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using Volo.Blogging.EntityFrameworkCore;

namespace Volo.Blogging.Posts
{
    public class EfCorePostRepository : EfCoreRepository<IBloggingDbContext, Post, Guid>, IPostRepository
    {
        public EfCorePostRepository(IDbContextProvider<IBloggingDbContext> dbContextProvider)
            : base(dbContextProvider)
        {

        }

        public async Task<List<Post>> GetPostsByBlogId(Guid id)
        {
            return await DbSet.Where(p => p.BlogId == id).OrderByDescending(p=>p.CreationTime).ToListAsync();
        }

        public async Task<Post> GetPostByUrl(Guid blogId, string url)
        {
            var post = await DbSet.FirstOrDefaultAsync(p => p.BlogId == blogId && p.Url == url);

            if (post == null)
            {
                throw new EntityNotFoundException(typeof(Post), nameof(post));
            }

            return post;
        }

        public override IQueryable<Post> WithDetails()
        {
            return GetQueryable().IncludeDetails();
        }
    }
}
