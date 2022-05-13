using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Volo.Abp.Caching;

namespace Volo.Abp.OpenIddict;

public class AbpOpenIddictCacheBase<TEntity, TModel, TStore>
    where TModel : class
    where TEntity : class
{
    public ILogger<AbpOpenIddictCacheBase<TEntity, TModel, TStore>> Logger { get; set; }

    protected IDistributedCache<TModel> Cache { get; }

    protected IDistributedCache<TModel[]> ArrayCache { get; }

    protected TStore Store { get; }

    protected AbpOpenIddictCacheBase(IDistributedCache<TModel> cache, IDistributedCache<TModel[]> arrayCache, TStore store)
    {
        Cache = cache;
        ArrayCache = arrayCache;
        Store = store;

        Logger = NullLogger<AbpOpenIddictCacheBase<TEntity, TModel, TStore>>.Instance;
    }
}
