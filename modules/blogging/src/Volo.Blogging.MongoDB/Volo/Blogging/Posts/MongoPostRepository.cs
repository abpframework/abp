using System;
using System.Collections.Generic;
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

        public async Task<List<Post>> GetPostsByBlogId(Guid id)
        {
            return await (await GetMongoQueryableAsync()).Where(p => p.BlogId == id).OrderByDescending(p => p.CreationTime).ToListAsync();
        }


        public async Task<bool> IsPostUrlInUseAsync(Guid blogId, string url, Guid? excludingPostId = null)
        {
            var query = (await GetMongoQueryableAsync()).Where(p => blogId == p.BlogId && p.Url == url);

            if (excludingPostId != null)
            {
                query = query.Where(p => excludingPostId != p.Id);
            }

            return await query.AnyAsync();
        }

        public async Task<Post> GetPostByUrl(Guid blogId, string url)
        {
            var post = await (await GetMongoQueryableAsync()).FirstOrDefaultAsync(p => p.BlogId == blogId && p.Url == url);

            if (post == null)
            {
                throw new EntityNotFoundException(typeof(Post), nameof(post));
            }

            return post;
        }

        public async Task<List<Post>> GetOrderedList(Guid blogId, bool @descending = false)
        {
            var query =  (await GetMongoQueryableAsync()).Where(x => x.BlogId == blogId);

            if (!descending)
            {
                return await query.OrderBy(x => x.CreationTime).ToListAsync();
            }

            return await query.OrderByDescending(x => x.CreationTime).ToListAsync();
        }
    }
}
