using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Shouldly;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Security.Claims;
using Xunit;

namespace Volo.Abp.Permissions
{
    public class PermissionChecker_User_Tests : PermissionTestBase
    {
        private readonly IPermissionChecker _permissionChecker;

        public PermissionChecker_User_Tests()
        {
            _permissionChecker = GetRequiredService<IPermissionChecker>();
        }

        [Fact]
        public async Task Should_Return_True_For_Granted_Current_User()
        {
            (await _permissionChecker.IsGrantedAsync(
                CreatePrincipal(PermissionTestDataBuilder.User1Id),
                "MyPermission1"
            )).ShouldBeTrue();
        }

        [Fact]
        public async Task Should_Return_False_For_Non_Granted_Current_User()
        {
            (await _permissionChecker.IsGrantedAsync(
                CreatePrincipal(PermissionTestDataBuilder.User2Id),
                "MyPermission1"
            )).ShouldBeFalse();
        }

        [Fact]
        public async Task Should_Return_False_For_Current_User_If_Anonymous()
        {
            (await _permissionChecker.IsGrantedAsync(
                CreatePrincipal(null),
                "MyPermission1"
            )).ShouldBeFalse();
        }

        private static ClaimsPrincipal CreatePrincipal(Guid? userId)
        {
            var claimsIdentity = new ClaimsIdentity();

            if (userId != null)
            {
                claimsIdentity.AddClaim(new Claim(AbpClaimTypes.UserId, userId.ToString()));
            }

            return new ClaimsPrincipal(claimsIdentity);
        }
    }
}
