using System;

namespace Volo.CmsKit.Admin.RobotsTxts;

[Serializable]
public class RobotsTxtDto
{
    /// <summary>
    /// The custom or inherited content. Null if <see cref="Source"/> is <see cref="RobotsTxtSource.Default"/>.
    /// </summary>
    public string Content { get; set; }

    public RobotsTxtSource Source { get; set; }
}
