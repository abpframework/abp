using System.Collections.Generic;

namespace Volo.Abp.AspNetCore.Security.Claims
{
    public class AbpClaimsMapOptions
    {
        public Dictionary<string, string> Map { get; }

        public AbpClaimsMapOptions()
        {
            Map = new Dictionary<string, string>();
        }
    }
}
