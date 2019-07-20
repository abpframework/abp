using System.Collections.Generic;
using System.Security.Claims;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Security.Claims;

namespace DashboardDemo.Security
{
    [Dependency(ReplaceServices = true)]
    public class FakeCurrentPrincipalAccessor : ICurrentPrincipalAccessor, ISingletonDependency
    {
        public ClaimsPrincipal Principal => GetPrincipal();
        private ClaimsPrincipal _principal;

        private ClaimsPrincipal GetPrincipal()
        {
            if (_principal == null)
            {
                lock (this)
                {
                    if (_principal == null)
                    {
                        _principal = new ClaimsPrincipal(
                            new ClaimsIdentity(
                                new List<Claim>
                                {
                                    new Claim(AbpClaimTypes.UserId,"2e701e62-0953-4dd3-910b-dc6cc93ccb0d"),
                                    new Claim(AbpClaimTypes.UserName,"admin"),
                                    new Claim(AbpClaimTypes.Email,"admin@abp.io")
                                }
                            )
                        );
                    }
                }
            }

            return _principal;
        }
    }
}