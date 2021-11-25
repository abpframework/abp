using System;
using JetBrains.Annotations;

namespace Volo.Abp.IdentityServer.ApiResources;

[Serializable]
public class ApiResourceEto
{
    public Guid Id { get; set; }

    [NotNull]
    public string Name { get; set; }

    public string DisplayName { get; set; }

    public string Description { get; set; }

    public bool Enabled { get; set; }
}
