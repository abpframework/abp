using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Volo.Abp.AspNetCore.Security;

public class AbpSecurityHeadersOptions
{
    public bool UseContentSecurityPolicyHeader { get; set; }
    
    public bool UseContentSecurityPolicyNonce { get; set; }

    public Dictionary<string, IEnumerable<string>> ContentSecurityPolicyValues { get; }

    public Dictionary<string, string> Headers { get; }
    
    public List<Func<HttpContext, Task<bool>>> IgnoredNonceScriptSelectors { get; }
    
    public List<string> IgnoredNonceScriptPaths { get; }

    public AbpSecurityHeadersOptions()
    {
        Headers = new Dictionary<string, string>();
        ContentSecurityPolicyValues = new Dictionary<string, IEnumerable<string>>();
        IgnoredNonceScriptSelectors = new List<Func<HttpContext, Task<bool>>>();
        IgnoredNonceScriptPaths = new List<string>();
    }
}
