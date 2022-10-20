using OpenIddict.Abstractions;

namespace Volo.Abp.OpenIddict.Applications;

public class AbpApplicationDescriptor : OpenIddictApplicationDescriptor
{
    /// <summary>
    /// URI to further information about client.
    /// </summary>
    public string ClientUri { get; set; }

    /// <summary>
    /// URI to client logo.
    /// </summary>
    public string LogoUri { get; set; }
}
