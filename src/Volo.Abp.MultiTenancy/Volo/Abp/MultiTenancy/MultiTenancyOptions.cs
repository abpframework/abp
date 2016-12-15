using System.Collections.Generic;

namespace Volo.Abp.MultiTenancy
{
    public class MultiTenancyOptions
    {
        public List<ITenantResolver> TenantResolvers { get; }

        public MultiTenancyOptions()
        {
            TenantResolvers = new List<ITenantResolver>();
        }
    }
}