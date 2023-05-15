using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Volo.Abp.AspNetCore.Security;

public class AbpSecurityHeadersOptions
{
    public bool UseContentSecurityPolicyHeader { get; set; }
    
    public bool UseContentSecurityPolicyNonce { get; set; }

    public Dictionary<string,IEnumerable<string>> ContentSecurityPolicyValueDictionary { get; }

    public Dictionary<string, string> Headers { get; }
    
    public List<Func<HttpContext, Task<bool>>> AlwaysIgnoreSecurityHeadersSelectors { get; }

    public AbpSecurityHeadersOptions()
    {
        Headers = new Dictionary<string, string>();
        ContentSecurityPolicyValueDictionary = new Dictionary<string, IEnumerable<string>>();
        AlwaysIgnoreSecurityHeadersSelectors = new List<Func<HttpContext, Task<bool>>>();
    }
}
