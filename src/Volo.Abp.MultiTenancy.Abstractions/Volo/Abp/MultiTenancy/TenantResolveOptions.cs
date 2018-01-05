using System.Collections.Generic;
using JetBrains.Annotations;

namespace Volo.Abp.MultiTenancy
{
    public class TenantResolveOptions
    {
        [NotNull]
        public List<ITenantResolver> TenantResolvers { get; }

        public TenantResolveOptions()
        {
            TenantResolvers = new List<ITenantResolver>();
        }
    }
}