using System.Collections.Generic;

namespace Volo.Abp.AspNetCore.Security;

public class AbpSecurityHeadersOptions
{
    public bool UseContentSecurityPolicyHeader { get; set; }

    public string ContentSecurityPolicyValue { get; set; }

    public Dictionary<string, string> Headers { get; }

    public AbpSecurityHeadersOptions()
    {
        Headers = new Dictionary<string, string>();
    }
}
