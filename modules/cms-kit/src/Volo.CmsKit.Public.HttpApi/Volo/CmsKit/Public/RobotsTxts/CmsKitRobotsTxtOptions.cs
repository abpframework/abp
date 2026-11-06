namespace Volo.CmsKit.Public.RobotsTxts;

public class CmsKitRobotsTxtOptions
{
    /// <summary>
    /// Maps the "/robots.txt" endpoint in this application.
    /// Set it to false in applications that should not serve the managed robots.txt content,
    /// like an admin portal or an API host that is not exposed to the crawlers.
    /// Default: true.
    /// </summary>
    public bool IsEndpointEnabled { get; set; } = true;

    /// <summary>
    /// The content served at /robots.txt when neither the current tenant nor the host has stored a content
    /// and there is no static robots.txt file in the web root folder.
    /// Default: "User-agent: *\nAllow: /" (allows crawling the whole site).
    /// </summary>
    public string DefaultContent { get; set; } = "User-agent: *\nAllow: /";

    /// <summary>
    /// The name of the static file in the web root folder (wwwroot) that is used as the default content, if exists.
    /// Default: "robots.txt".
    /// </summary>
    public string StaticFileName { get; set; } = "robots.txt";
}
