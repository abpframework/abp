using System;
using System.Security.Claims;

namespace Volo.Abp.Security.Claims
{
    public class ClaimsIdentityContext
    {
        public ClaimsIdentity ClaimsIdentity { get; }

        public IServiceProvider ServiceProvider { get; }

        public ClaimsIdentityContext(
            ClaimsIdentity claimsIdentity,
            IServiceProvider serviceProvider)
        {
            ClaimsIdentity = claimsIdentity;
            ServiceProvider = serviceProvider;
        }
    }
}
