using System.Collections.Generic;

namespace Volo.Docs.Seo
{
    public class SitemapOptions
    {
        public List<SitemapUrlOptions> AdditionalSitemapItems { get; set; }

        public SitemapOptions()
        {
            AdditionalSitemapItems = new();
        }
    }
}