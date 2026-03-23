using System;

namespace Volo.Abp.OpenIddict.Applications;

[Serializable]
public class OpenIddictApplicationClientIdChangedEto
{
    public Guid Id { get; set; }

    public string ClientId { get; set; }

    public string OldClientId { get; set; }
}
