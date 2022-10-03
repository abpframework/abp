using System.Collections.Generic;

namespace Volo.Abp.OpenIddict.WildcardDomains;

public class AbpOpenIddictWildcardDomainOptions
{
    public bool EnableWildcardDomainSupport { get; set; }

    public HashSet<string> WildcardDomainsFormat { get; }

    public string WildcardDomainPlaceholder { get; set; }

    public AbpOpenIddictWildcardDomainOptions()
    {
        WildcardDomainsFormat = new HashSet<string>();
        WildcardDomainPlaceholder = "___";
    }

}
