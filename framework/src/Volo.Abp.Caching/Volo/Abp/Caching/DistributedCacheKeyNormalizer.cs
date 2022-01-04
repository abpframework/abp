using Microsoft.Extensions.Options;
using Volo.Abp.DependencyInjection;
using Volo.Abp.MultiTenancy;

namespace Volo.Abp.Caching;

public class DistributedCacheKeyNormalizer : IDistributedCacheKeyNormalizer, ITransientDependency
{
    protected ICurrentTenant CurrentTenant { get; }

    protected AbpDistributedCacheOptions DistributedCacheOptions { get; }

    public DistributedCacheKeyNormalizer(
        ICurrentTenant currentTenant,
        IOptions<AbpDistributedCacheOptions> distributedCacheOptions)
    {
        CurrentTenant = currentTenant;
        DistributedCacheOptions = distributedCacheOptions.Value;
    }

    public virtual string NormalizeKey(DistributedCacheKeyNormalizeArgs args)
    {
        var normalizedKey = $"c:{args.CacheName},k:{DistributedCacheOptions.KeyPrefix}{args.Key}";

        if (!args.IgnoreMultiTenancy && CurrentTenant.Id.HasValue)
        {
            normalizedKey = $"t:{CurrentTenant.Id.Value},{normalizedKey}";
        }

        return normalizedKey;
    }
}
