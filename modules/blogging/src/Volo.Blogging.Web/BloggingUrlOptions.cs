using System;
using System.Collections.Generic;

namespace Volo.Blogging
{
    public class BloggingUrlOptions
    {
        private string _routePrefix = "blog";

        /// <summary>
        /// Default value: "blog";
        /// </summary>
        public string RoutePrefix
        {
            get => GetFormattedRoutePrefix();
            set => _routePrefix = value;
        }

        /// <summary>
        /// Used to specify ignore paths if the route prefix is null or empty.
        /// </summary>
        public List<string> IgnoredPaths { get; } = new ();

        private string GetFormattedRoutePrefix()
        {
            if (string.IsNullOrWhiteSpace(_routePrefix))
            {
                return "/";
            }

            return _routePrefix.EnsureEndsWith('/').EnsureStartsWith('/');
        }
    }
}
