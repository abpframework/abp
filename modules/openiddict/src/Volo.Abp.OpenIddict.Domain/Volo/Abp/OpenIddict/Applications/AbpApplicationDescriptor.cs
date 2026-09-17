using System;
using OpenIddict.Abstractions;

namespace Volo.Abp.OpenIddict.Applications;

public class AbpApplicationDescriptor : OpenIddictApplicationDescriptor
{
    /// <summary>
    /// Gets or sets the front-channel logout URI associated with the application.
    /// </summary>
    public virtual Uri FrontChannelLogoutUri { get; set; }

    /// <summary>
    /// URI to further information about client.
    /// </summary>
    public virtual string ClientUri { get; set; }

    /// <summary>
    /// URI to client logo.
    /// </summary>
    public virtual string LogoUri { get; set; }
}
