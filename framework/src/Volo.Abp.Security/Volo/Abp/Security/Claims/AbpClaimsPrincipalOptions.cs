using System.Collections.Generic;
using Volo.Abp.Collections;

namespace Volo.Abp.Security.Claims
{
    public class AbpClaimsPrincipalOptions
    {
        public ITypeList<IAbpClaimsPrincipalContributor> Contributors { get; }

        public List<string> RequestedClaims { get; }

        public AbpClaimsPrincipalOptions()
        {
            Contributors = new TypeList<IAbpClaimsPrincipalContributor>();
            RequestedClaims = new List<string>();
        }
    }
}
