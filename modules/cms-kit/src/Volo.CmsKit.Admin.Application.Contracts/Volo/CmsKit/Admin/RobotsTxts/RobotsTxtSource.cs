namespace Volo.CmsKit.Admin.RobotsTxts;

public enum RobotsTxtSource
{
    /// <summary>
    /// The current tenant (or host) has its own content.
    /// </summary>
    Custom = 0,

    /// <summary>
    /// The current tenant has no content and uses the host's content.
    /// </summary>
    InheritedFromHost = 1,

    /// <summary>
    /// Neither the current tenant nor the host has content.
    /// The UI serves the default content (the static wwwroot/robots.txt file or the configured default).
    /// </summary>
    Default = 2
}
