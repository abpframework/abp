using System;
using System.Threading;
using System.Threading.Tasks;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using JetBrains.Annotations;
using Volo.Abp;
using Volo.Abp.Domain.Repositories.MongoDB;
using Volo.Abp.MongoDB;
using Volo.CmsKit.Blogs;

namespace Volo.CmsKit.MongoDB.Blogs
{
    public class MongoBlogRepository : MongoDbRepository<ICmsKitMongoDbContext, Blog, Guid>, IBlogRepository
    {
        public MongoBlogRepository(IMongoDbContextProvider<ICmsKitMongoDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }

        public virtual async Task<bool> ExistsAsync(Guid blogId, CancellationToken cancellationToken = default)
        {
            return await AsyncExecuter.AnyAsync(
                            await GetQueryableAsync(),
                                x => x.Id == blogId, 
                                cancellationToken);
        }

        public virtual async Task<bool> HasPostsAsync(Guid blogId, CancellationToken cancellationToken = default)
        {
            var token = GetCancellationToken(cancellationToken);

            var queryable = (await GetDbContextAsync(token)).Collection<BlogPost>().AsQueryable();

            return await queryable.AnyAsync(x => x.BlogId == blogId, token);
        }

        public virtual Task<Blog> GetBySlugAsync([NotNull]string slug, CancellationToken cancellationToken = default)
        {
            Check.NotNullOrEmpty(slug, nameof(slug));
            return GetAsync(x => x.Slug == slug, cancellationToken: cancellationToken);
        }
    }
}
