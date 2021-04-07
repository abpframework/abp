using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Caching;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EventBus;

namespace Volo.Blogging.Posts
{
    public class PostCacheInvalidator : ILocalEventHandler<PostChangedEvent>, ITransientDependency
    {
        protected IDistributedCache<List<PostCacheItem>> Cache { get; }

        public PostCacheInvalidator(IDistributedCache<List<PostCacheItem>> cache)
        {
            Cache = cache;
        }
        
        public virtual async Task HandleEventAsync(PostChangedEvent post)
        {
            await Cache.RemoveAsync(post.BlogId.ToString());
        }
    }
}