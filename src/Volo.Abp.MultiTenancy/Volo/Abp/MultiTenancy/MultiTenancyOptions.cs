using System.Collections.Generic;
using JetBrains.Annotations;

namespace Volo.Abp.MultiTenancy
{
    public class MultiTenancyOptions
    {
        [NotNull]
        public List<ITenantResolver> TenantResolvers { get; }

        public MultiTenancyOptions()
        {
            TenantResolvers = new List<ITenantResolver>();
        }
    }
}