using System.Collections.Generic;

namespace Volo.Docs.Seo
{
    public class DocsSeoOptions
    {
        /// <summary>
        /// Default: false
        /// </summary>
        public bool IsEnabled { get; set; } = false;

        public List<RobotsTxtOptions> RobotsTxt { get; set; }

        public SitemapOptions Sitemap { get; set; }

        public DocsSeoOptions()
        {
            RobotsTxt = new();
            Sitemap = new();
        }
    }
}