using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Caching;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Entities.Events;
using Volo.Abp.EventBus;

namespace Volo.Blogging.Posts
{
    public class PostCacheInvalidator : ILocalEventHandler<EntityChangedEventData<Post>>, ITransientDependency
    {
        protected IDistributedCache<List<PostCacheItem>> Cache { get; }

        public PostCacheInvalidator(IDistributedCache<List<PostCacheItem>> cache)
        {
            Cache = cache;
        }
        
        public virtual async Task HandleEventAsync(EntityChangedEventData<Post> eventData)
        {
            var cacheKey = eventData.Entity.BlogId.ToString();
            await Cache.RemoveAsync(cacheKey);
        }
    }
}