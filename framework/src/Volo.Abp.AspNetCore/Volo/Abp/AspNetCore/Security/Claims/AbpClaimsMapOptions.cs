using System;
using System.Collections.Generic;
using Volo.Abp.Security.Claims;

namespace Volo.Abp.AspNetCore.Security.Claims
{
    public class AbpClaimsMapOptions
    {
        public Dictionary<string, Func<string>> Maps { get; }

        public AbpClaimsMapOptions()
        {
            Maps = new Dictionary<string, Func<string>>()
            {
                { "sub", () => AbpClaimTypes.UserId },
                { "role", () => AbpClaimTypes.Role },
                { "email", () => AbpClaimTypes.Email },
                { "name", () => AbpClaimTypes.UserName },
                { "family_name", () => AbpClaimTypes.SurName },
                { "given_name", () => AbpClaimTypes.Name }
            };
        }
    }
}
