using System.Collections.Generic;
using Volo.Abp.Security.Claims;

namespace Volo.Abp.Identity.AspNetCore;

public class AbpRefreshingPrincipalOptions
{
    public List<string> CurrentPrincipalKeepClaimTypes { get; set; }

    public AbpRefreshingPrincipalOptions()
    {
        CurrentPrincipalKeepClaimTypes = new List<string>
        {
            AbpClaimTypes.SessionId
        };
    }
}
