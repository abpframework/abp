﻿using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Shouldly;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Security.Claims;
using Xunit;

namespace Volo.Abp.PermissionManagement
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
            ).ConfigureAwait(false)).ShouldBeTrue();
        }

        [Fact]
        public async Task Should_Return_False_For_Non_Granted_Current_User()
        {
            (await _permissionChecker.IsGrantedAsync(
                CreatePrincipal(PermissionTestDataBuilder.User2Id),
                "MyPermission1"
            ).ConfigureAwait(false)).ShouldBeFalse();
        }

        [Fact]
        public async Task Should_Return_False_For_Current_User_If_Anonymous()
        {
            (await _permissionChecker.IsGrantedAsync(
                CreatePrincipal(null),
                "MyPermission1"
            ).ConfigureAwait(false)).ShouldBeFalse();
        }

        [Fact]
        public async Task Should_Not_Allow_Host_Permission_To_Tenant_User_Even_Granted_Before()
        {
            (await _permissionChecker.IsGrantedAsync(
                CreatePrincipal(PermissionTestDataBuilder.User1Id, Guid.NewGuid()),
                "MyPermission3"
            ).ConfigureAwait(false)).ShouldBeFalse();
        }

        private static ClaimsPrincipal CreatePrincipal(Guid? userId, Guid? tenantId = null)
        {
            var claimsIdentity = new ClaimsIdentity();

            if (userId != null)
            {
                claimsIdentity.AddClaim(new Claim(AbpClaimTypes.UserId, userId.ToString()));
            }

            if (tenantId != null)
            {
                claimsIdentity.AddClaim(new Claim(AbpClaimTypes.TenantId, tenantId.ToString()));
            }

            return new ClaimsPrincipal(claimsIdentity);
        }
    }
}
