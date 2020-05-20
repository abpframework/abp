using System.Security.Claims;
using System.Threading.Tasks;
using Shouldly;
using Volo.Abp.AspNetCore.Mvc.Authorization;
using Volo.Abp.AspNetCore.TestBase;
using Volo.Abp.Autofac;
using Volo.Abp.MemoryDb;
using Volo.Abp.Modularity;
using Xunit;

namespace Volo.Abp.AspNetCore.Mvc.Security.Claims
{
    [DependsOn(
        typeof(AbpAspNetCoreTestBaseModule),
        typeof(AbpMemoryDbTestModule),
        typeof(AbpAspNetCoreMvcModule),
        typeof(AbpAutofacModule)
    )]
    public class ClaimsMapTestController_Tests : AspNetCoreMvcTestBase
    {
        private readonly FakeUserClaims _fakeRequiredService;

        public ClaimsMapTestController_Tests()
        {
            _fakeRequiredService = GetRequiredService<FakeUserClaims>();
        }

        [Fact]
        public async Task Claims_Should_Be_Mapped()
        {
            _fakeRequiredService.Claims.AddRange(new[]
            {
                new Claim("SerialNumber", "123456"),
                new Claim("DateOfBirth", "2020")
            });

            var result = await GetResponseAsStringAsync("/ClaimsMapTest/ClaimsMapTest");
            result.ShouldBe("OK");
        }
    }
}
