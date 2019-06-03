using System;

namespace Volo.Docs
{
    public class DocsUrlOptions
    {
        /// <summary>
        /// Default value: "documents";
        /// </summary>
        public string RoutePrefix { get; set; } = "documents";

        public string GetFormattedRoutePrefix()
        {
                if (string.IsNullOrWhiteSpace(RoutePrefix))
                {
                    return "";
                }

                return RoutePrefix.Trim('/').EnsureEndsWith('/');
        }
    }
}
