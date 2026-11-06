using System;

namespace Volo.CmsKit.Public.RobotsTxts;

[Serializable]
public class RobotsTxtPublicDto
{
    /// <summary>
    /// The content of the current tenant, or the host's content if the tenant has none.
    /// Null if neither has stored content; the UI then serves its default content.
    /// </summary>
    public string Content { get; set; }
}
