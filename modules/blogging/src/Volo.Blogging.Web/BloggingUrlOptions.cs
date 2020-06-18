using System;

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
