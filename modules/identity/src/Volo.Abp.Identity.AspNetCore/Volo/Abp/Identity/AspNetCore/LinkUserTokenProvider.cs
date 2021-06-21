using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Volo.Abp.Identity.AspNetCore
{
    public class LinkUserTokenProvider : DataProtectorTokenProvider<IdentityUser>
    {
        public LinkUserTokenProvider(
            IDataProtectionProvider dataProtectionProvider,
            IOptions<DataProtectionTokenProviderOptions> options,
            ILogger<DataProtectorTokenProvider<IdentityUser>> logger)
            : base(dataProtectionProvider, options, logger)
        {

        }
    }
}
