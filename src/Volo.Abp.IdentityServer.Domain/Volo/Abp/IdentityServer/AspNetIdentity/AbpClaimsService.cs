using IdentityServer4.Services;
using Microsoft.Extensions.Logging;

namespace Volo.Abp.IdentityServer.AspNetIdentity
{
    public class AbpClaimsService : DefaultClaimsService
    {
        public AbpClaimsService(IProfileService profile, ILogger<DefaultClaimsService> logger)
            : base(profile, logger)
        {
        }
    }
}
