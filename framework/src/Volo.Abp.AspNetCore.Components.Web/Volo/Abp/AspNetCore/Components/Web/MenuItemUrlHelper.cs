using System;

namespace Volo.Abp.AspNetCore.Components.Web;

public static class MenuItemUrlHelper
{
    /// <summary>
    /// Makes an app-local menu url ("/x" or "~/x") relative to the base href, so it keeps the PathBase.
    /// Protocol-relative urls ("//host/x") point to another host and are returned as is.
    /// </summary>
    public static string? ToBaseRelativeUrl(string? url)
    {
        if (url == null || url.StartsWith("//", StringComparison.Ordinal))
        {
            return url;
        }

        return url.TrimStart('/', '~');
    }
}
