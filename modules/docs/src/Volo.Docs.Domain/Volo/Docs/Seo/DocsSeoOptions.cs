using System;
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

    public class RobotsTxtOptions
    {
        public string UserAgent { get; set; }
        
        public List<string> DisallowUrls { get; set; }

        public List<string> AllowUrls { get; set; }

        public RobotsTxtOptions()
        {
            DisallowUrls = new ();
            AllowUrls = new();
        }
    }

    public class SitemapOptions
    {
        public List<SitemapUrlOptions> AdditionalSitemapItems { get; set; }

        public SitemapOptions()
        {
            AdditionalSitemapItems = new();
        }
    }

    public class SitemapUrlOptions
    {
        public string Url { get; set; }

        public DateTime? ModifiedDate { get; set; }

        public double? Priority { get; set; }
    }
}