using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
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

        public async Task<List<Post>> GetPostsByBlogId(Guid id, CancellationToken cancellationToken = default)
        {
            return await (await GetDbSetAsync()).Where(p => p.BlogId == id).OrderByDescending(p=>p.CreationTime).ToListAsync(GetCancellationToken(cancellationToken));
        }

        public async Task<bool> IsPostUrlInUseAsync(Guid blogId, string url, Guid? excludingPostId = null, CancellationToken cancellationToken = default)
        {
            var query = (await GetDbSetAsync()).Where(p => blogId == p.BlogId && p.Url == url);

            if (excludingPostId != null)
            {
                query = query.Where(p => excludingPostId != p.Id);
            }

            return await query.AnyAsync(GetCancellationToken(cancellationToken));
        }

        public async Task<Post> GetPostByUrl(Guid blogId, string url, CancellationToken cancellationToken = default)
        {
            var post = await (await GetDbSetAsync()).FirstOrDefaultAsync(p => p.BlogId == blogId && p.Url == url, GetCancellationToken(cancellationToken));

            if (post == null)
            {
                throw new EntityNotFoundException(typeof(Post), nameof(post));
            }

            return post;
        }

        public async Task<List<Post>> GetOrderedList(Guid blogId,bool descending = false, CancellationToken cancellationToken = default)
        {
            if (!descending)
            {
                return await (await GetDbSetAsync()).Where(x=>x.BlogId==blogId).OrderByDescending(x => x.CreationTime).ToListAsync(GetCancellationToken(cancellationToken));
            }
            else
            {
                return await (await GetDbSetAsync()).Where(x => x.BlogId == blogId).OrderBy(x => x.CreationTime).ToListAsync(GetCancellationToken(cancellationToken));
            }

        }

        public override async Task<IQueryable<Post>> WithDetailsAsync()
        {
            return (await GetQueryableAsync()).IncludeDetails();
        }
    }
}
