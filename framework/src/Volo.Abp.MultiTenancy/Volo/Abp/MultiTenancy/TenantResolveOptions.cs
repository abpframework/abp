using System.Collections.Generic;
using JetBrains.Annotations;

namespace Volo.Abp.MultiTenancy
{
    public class TenantResolveOptions
    {
        [NotNull]
        public List<ITenantResolveContributor> TenantResolvers { get; }

        public TenantResolveOptions()
        {
            TenantResolvers = new List<ITenantResolveContributor>
            {
                new CurrentUserTenantResolveContributor()
            };
        }
    }
}