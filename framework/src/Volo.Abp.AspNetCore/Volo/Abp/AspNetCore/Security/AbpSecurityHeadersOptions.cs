using System.Collections.Generic;

namespace Volo.Abp.AspNetCore.Security;

public class AbpSecurityHeadersOptions
{
    public bool UseContentSecurityPolicyHeader { get; set; }
    
    public bool UseContentSecurityPolicyNonce { get; set; }

    public Dictionary<string,IEnumerable<string>> ContentSecurityPolicyValueDictionary { get; }

    public Dictionary<string, string> Headers { get; }
    
    public List<string> IgnoredUrls { get; }

    public AbpSecurityHeadersOptions()
    {
        Headers = new Dictionary<string, string>();
        ContentSecurityPolicyValueDictionary = new Dictionary<string, IEnumerable<string>>();
        IgnoredUrls = new List<string> ();
    }
}
