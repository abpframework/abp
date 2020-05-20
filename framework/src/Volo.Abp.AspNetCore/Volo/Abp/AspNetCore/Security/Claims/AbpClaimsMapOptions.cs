using System.Collections.Generic;
using Volo.Abp.Security.Claims;

namespace Volo.Abp.AspNetCore.Security.Claims
{
    public class AbpClaimsMapOptions
    {
        public Dictionary<string, string> Maps { get; }

        public AbpClaimsMapOptions()
        {
            Maps = new Dictionary<string, string>()
            {
                { "sub", AbpClaimTypes.UserId },
                { "role", AbpClaimTypes.Role },
                { "email", AbpClaimTypes.Email },
            };
        }
    }
}
