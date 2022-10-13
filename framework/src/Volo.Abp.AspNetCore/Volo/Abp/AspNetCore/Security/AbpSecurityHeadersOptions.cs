namespace Volo.Abp.AspNetCore.Security;

public class AbpSecurityHeadersOptions
{
    public bool UseContentSecurityPolicyHeader { get; set; }

    public string ContentSecurityPolicyValue { get; set; }
}
