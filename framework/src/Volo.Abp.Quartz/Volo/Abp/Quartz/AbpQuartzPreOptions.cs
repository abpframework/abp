using System.Collections.Specialized;

namespace Volo.Abp.Quartz
{
    public class AbpQuartzPreOptions
    {
        public NameValueCollection Properties { get; set; }

        public AbpQuartzPreOptions()
        {
            Properties = new NameValueCollection();
        }
    }
}
