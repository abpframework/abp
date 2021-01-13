using System;
using System.Security.Claims;
using JetBrains.Annotations;

namespace Volo.Abp.Security.Claims
{
    public class ClaimsIdentityContext
    {
        [NotNull]
        public ClaimsIdentity ClaimsIdentity { get; }

        [NotNull]
        public IServiceProvider ServiceProvider { get; }

        public ClaimsIdentityContext(
            [NotNull] ClaimsIdentity claimsIdentity,
            [NotNull] IServiceProvider serviceProvider)
        {
            ClaimsIdentity = claimsIdentity;
            ServiceProvider = serviceProvider;
        }
    }
}
