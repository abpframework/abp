using System.Collections.Generic;

namespace Volo.Abp.AspNetCore.Mvc.UI.Layout;

public class PageEmbeddingOptions
{
    /// <summary>
    /// Query parameter name to enable embedding mode. Default: "embed"
    /// </summary>
    public string QueryParameterName { get; set; } = "embed";

    /// <summary>
    /// Query parameter values that enable embedding mode. Default: ["true", "1"]
    /// </summary>
    public HashSet<string> QueryParameterValues { get; set; } = new() { "true", "1" };

    /// <summary>
    /// Paths that should always be rendered without layout elements (for embedding)
    /// </summary>
    public HashSet<string> EmbeddedPaths { get; set; } = new();

    /// <summary>
    /// Path patterns that should always be rendered without layout elements (for embedding)
    /// Supports wildcards like "/api/embed/*"
    /// </summary>
    public HashSet<string> EmbeddedPathPatterns { get; set; } = new();

    /// <summary>
    /// When true, automatically disable layout elements for requests that come from iframes.
    /// Uses multiple detection methods with fallbacks.
    /// </summary>
    public bool AlwaysEmbedIFrameRequests { get; set; } = false;
} 