using System;

namespace Volo.Docs.Seo
{
    internal class SitemapUrl
    {
        public string Url { get; set; }
        public DateTime? Modified { get; set; }
        public double? Priority { get; set; }
    }
}