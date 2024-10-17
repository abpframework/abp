using System.Collections.Generic;

namespace Volo.Abp.OpenIddict.WildcardDomains;

public class AbpOpenIddictWildcardDomainOptions
{
    public bool EnableWildcardDomainSupport { get; set; }

    /// <summary>
    /// Wildcard domains format. For example: https://*.abp.io
    /// </summary>
    public HashSet<string> WildcardDomainsFormat { get; }

    public AbpOpenIddictWildcardDomainOptions()
    {
        WildcardDomainsFormat = new HashSet<string>();
    }
}
