using System.Security.Claims;
using System.Threading.Tasks;
using Shouldly;
using Volo.Abp.AspNetCore.TestBase;
using Volo.Abp.Autofac;
using Volo.Abp.MemoryDb;
using Volo.Abp.Modularity;
using Volo.Abp.Security.Claims;
using Xunit;

namespace Volo.Abp.AspNetCore.Mvc.Authorization
{
    [DependsOn(
        typeof(AbpAspNetCoreTestBaseModule),
        typeof(AbpMemoryDbTestModule),
        typeof(AbpAspNetCoreMvcModule),
        typeof(AbpAutofacModule)
    )]
    public class AuthTestPage_Tests: AspNetCoreMvcTestBase
    {
        private readonly FakeUserClaims _fakeRequiredService;

        public AuthTestPage_Tests()
        {
            _fakeRequiredService = GetRequiredService<FakeUserClaims>();
        }
        
        [Fact]
        public async Task Should_Call_Simple_Authorized_Method_With_Authenticated_User()
        {
            _fakeRequiredService.Claims.AddRange(new[]
            {
                new Claim(AbpClaimTypes.UserId, AuthTestController.FakeUserId.ToString())
            });

            var result = await GetResponseAsStringAsync("/Authorization/AuthTestPage");
            result.ShouldBe("OK");
        }
    }
}