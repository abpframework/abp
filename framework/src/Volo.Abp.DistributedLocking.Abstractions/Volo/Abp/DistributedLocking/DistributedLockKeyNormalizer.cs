using Microsoft.Extensions.Options;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.DistributedLocking;

public class DistributedLockKeyNormalizer : IDistributedLockKeyNormalizer, ITransientDependency
{
    protected AbpDistributedLockOptions Options { get; }
    
    public DistributedLockKeyNormalizer(IOptions<AbpDistributedLockOptions> options)
    {
        Options = options.Value;
    }
    
    public virtual string NormalizeKey(string name)
    {
        return $"{Options.KeyPrefix}{name}";
    }
}