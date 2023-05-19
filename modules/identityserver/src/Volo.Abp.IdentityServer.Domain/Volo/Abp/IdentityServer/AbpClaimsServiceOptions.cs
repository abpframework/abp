using System.Collections.Generic;

namespace Volo.Abp.IdentityServer;

public class AbpClaimsServiceOptions
{
    public List<string> RequestedClaims { get; }

    public AbpClaimsServiceOptions()
    {
        RequestedClaims = new List<string>();
    }
}
