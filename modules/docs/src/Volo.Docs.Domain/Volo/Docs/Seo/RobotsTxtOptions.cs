using System.Collections.Generic;

namespace Volo.Docs.Seo
{
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
}