using System.Data;

namespace Volo.Abp.OpenIddict;

public class AbpOpenIddictStoreOptions
{
    public IsolationLevel DefaultIsolationLevel { get; set; }

    public AbpOpenIddictStoreOptions()
    {
        DefaultIsolationLevel = IsolationLevel.RepeatableRead;
    }
}
