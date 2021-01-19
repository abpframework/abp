using System;

namespace Volo.CmsKit.Blogs
{
    public class CmsKitBlogOptions
    {
        private string routePrefix = "blog";

        /// <summary>
        /// Gets or sets route prefix. Default value is 'blog'.
        /// </summary>
        /// <remarks>
        /// This parameter will be used while presenting blogposts.
        /// </remarks>
        public string RoutePrefix
        {
            get => GetFormattedRoutePrefix();
            set => routePrefix = value;
        }

        private string GetFormattedRoutePrefix()
        {
            if (string.IsNullOrWhiteSpace(routePrefix))
            {
                return "/";
            }

            return routePrefix.EnsureEndsWith('/').EnsureStartsWith('/');
        }
    }
}
