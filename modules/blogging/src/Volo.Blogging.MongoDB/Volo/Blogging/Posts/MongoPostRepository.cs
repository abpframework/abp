using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories.MongoDB;
using Volo.Abp.MongoDB;
using Volo.Blogging.MongoDB;

namespace Volo.Blogging.Posts
{
    public class MongoPostRepository : MongoDbRepository<IBloggingMongoDbContext, Post, Guid>, IPostRepository
    {
        public MongoPostRepository(IMongoDbContextProvider<IBloggingMongoDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }

        public async Task<List<Post>> GetPostsByBlogId(Guid id, CancellationToken cancellationToken = default)
        {
            return await (await GetMongoQueryableAsync(cancellationToken)).Where(p => p.BlogId == id).OrderByDescending(p => p.CreationTime).ToListAsync(GetCancellationToken(cancellationToken));
        }


        public async Task<bool> IsPostUrlInUseAsync(Guid blogId, string url, Guid? excludingPostId = null, CancellationToken cancellationToken = default)
        {
            var query = (await GetMongoQueryableAsync(cancellationToken)).Where(p => blogId == p.BlogId && p.Url == url);

            if (excludingPostId != null)
            {
                query = query.Where(p => excludingPostId != p.Id);
            }

            return await query.AnyAsync(GetCancellationToken(cancellationToken));
        }

        public async Task<Post> GetPostByUrl(Guid blogId, string url, CancellationToken cancellationToken = default)
        {
            var post = await (await GetMongoQueryableAsync(cancellationToken)).FirstOrDefaultAsync(p => p.BlogId == blogId && p.Url == url, GetCancellationToken(cancellationToken));

            if (post == null)
            {
                throw new EntityNotFoundException(typeof(Post), nameof(post));
            }

            return post;
        }

        public async Task<List<Post>> GetOrderedList(Guid blogId, bool @descending = false, CancellationToken cancellationToken = default)
        {
            var query =  (await GetMongoQueryableAsync(cancellationToken)).Where(x => x.BlogId == blogId);

            if (!descending)
            {
                return await query.OrderBy(x => x.CreationTime).ToListAsync(GetCancellationToken(cancellationToken));
            }

            return await query.OrderByDescending(x => x.CreationTime).ToListAsync(GetCancellationToken(cancellationToken));
        }
    }
}
