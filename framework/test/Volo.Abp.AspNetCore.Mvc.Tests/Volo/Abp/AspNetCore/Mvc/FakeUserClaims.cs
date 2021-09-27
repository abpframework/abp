using System.Collections.Generic;
using System.Security.Claims;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.AspNetCore.Mvc
{
    public class FakeUserClaims : ISingletonDependency
    {
        public List<Claim> Claims { get; } = new List<Claim>();
    }
}
