using System.Collections.Generic;
using System.Security.Claims;
using Shouldly;
using Volo.Abp.Testing;
using Xunit;

namespace Volo.Abp.Security.Claims
{
    public class CurrentPrincipalAccessor_Test : AbpIntegratedTest<AbpSecurityTestModule>
    {
        private readonly ICurrentPrincipalAccessor _currentPrincipalAccessor;

        public CurrentPrincipalAccessor_Test()
        {
            _currentPrincipalAccessor = GetRequiredService<ICurrentPrincipalAccessor>();
        }

        [Fact]
        public void Should_Get_Changed_Principal_If()
        {
            var claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>
            {
                new Claim(ClaimTypes.Name,"bob"),
                new Claim(ClaimTypes.NameIdentifier,"123456")
            }));

            var claimsPrincipal2 = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>
            {
                new Claim(ClaimTypes.Name,"lee"),
                new Claim(ClaimTypes.NameIdentifier,"654321")
            }));


            _currentPrincipalAccessor.Principal.ShouldBe(null);

            using (_currentPrincipalAccessor.Change(claimsPrincipal))
            {
                _currentPrincipalAccessor.Principal.ShouldBe(claimsPrincipal);

                using (_currentPrincipalAccessor.Change(claimsPrincipal2))
                {
                    _currentPrincipalAccessor.Principal.ShouldBe(claimsPrincipal2);
                }

                _currentPrincipalAccessor.Principal.ShouldBe(claimsPrincipal);
            }
            _currentPrincipalAccessor.Principal.ShouldBeNull();
        }
    }
}
