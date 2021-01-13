using System.Security.Claims;

namespace Volo.Abp.Security.Claims
{
    public class ClaimsIdentityContext
    {
        public ClaimsIdentity ClaimsIdentity { get; }

        public ClaimsIdentityContext(ClaimsIdentity claimsIdentity)
        {
            ClaimsIdentity = claimsIdentity;
        }
    }
}
