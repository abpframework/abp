using System.Data;

namespace Volo.Abp.OpenIddict;

public class AbpOpenIddictStoreOptions
{
    public IsolationLevel? PruneIsolationLevel { get; set; }

    public IsolationLevel? DeleteIsolationLevel { get; set; }

    public AbpOpenIddictStoreOptions()
    {
        PruneIsolationLevel = IsolationLevel.RepeatableRead;
        DeleteIsolationLevel = IsolationLevel.Serializable;
    }
}
