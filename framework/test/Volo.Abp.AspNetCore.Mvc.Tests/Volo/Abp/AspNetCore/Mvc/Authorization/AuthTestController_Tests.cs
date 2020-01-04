﻿using System;
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
    public class AuthTestController_Tests : AspNetCoreMvcTestBase
    {
        private readonly FakeUserClaims _fakeRequiredService;

        public AuthTestController_Tests()
        {
            _fakeRequiredService = GetRequiredService<FakeUserClaims>();
        }

        [Fact]
        public async Task Should_Call_Anonymous_Method_Without_Authentication()
        {
            var result = await GetResponseAsStringAsync("/AuthTest/AnonymousTest").ConfigureAwait(false);
            result.ShouldBe("OK");
        }

        [Fact]
        public async Task Should_Call_Simple_Authorized_Method_With_Authenticated_User()
        {
            _fakeRequiredService.Claims.AddRange(new[]
            {
                new Claim(AbpClaimTypes.UserId, AuthTestController.FakeUserId.ToString())
            });

            var result = await GetResponseAsStringAsync("/AuthTest/SimpleAuthorizationTest").ConfigureAwait(false);
            result.ShouldBe("OK");
        }

        [Fact]
        public async Task Custom_Claim_Policy_Should_Work_With_Right_Claim_Provided()
        {
            _fakeRequiredService.Claims.AddRange(new[]
            {
                new Claim(AbpClaimTypes.UserId, AuthTestController.FakeUserId.ToString()),
                new Claim("MyCustomClaimType", "42")
            });

            var result = await GetResponseAsStringAsync("/AuthTest/CustomPolicyTest").ConfigureAwait(false);
            result.ShouldBe("OK");
        }

        [Fact]
        public async Task Custom_Claim_Policy_Should_Not_Work_With_Wrong_Claim_Value()
        {
            _fakeRequiredService.Claims.AddRange(new[]
            {
                new Claim(AbpClaimTypes.UserId, AuthTestController.FakeUserId.ToString()),
                new Claim("MyCustomClaimType", "43")
            });

            //TODO: We can get a real exception if we properly configure authentication schemas for this project
            await Assert.ThrowsAsync<InvalidOperationException>(async () =>
                await GetResponseAsStringAsync("/AuthTest/CustomPolicyTest")
.ConfigureAwait(false)).ConfigureAwait(false);
        }

        [Fact]
        public async Task Should_Authorize_For_Defined_And_Allowed_Permission()
        {
            _fakeRequiredService.Claims.AddRange(new[]
            {
                new Claim(AbpClaimTypes.UserId, AuthTestController.FakeUserId.ToString())
            });

            var result = await GetResponseAsStringAsync("/AuthTest/PermissionTest").ConfigureAwait(false);
            result.ShouldBe("OK");
        }
    }
}
