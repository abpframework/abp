using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using Shouldly;
using Volo.Abp.Session;
using Xunit;

namespace Volo.Abp.Permissions
{
    public class PermissionManager_User_Tests : AbpPermissionTestBase
    {
        private readonly IPermissionManager _permissionManager;

        private Guid? _currentUserId;

        public PermissionManager_User_Tests()
        {
            _permissionManager = GetRequiredService<IPermissionManager>();
        }

        protected override void AfterAddApplication(IServiceCollection services)
        {
            var currentUser = Substitute.For<ICurrentUser>();
            currentUser.Id.Returns(ci => _currentUserId);
            services.AddSingleton(currentUser);
        }

        [Fact]
        public async Task Should_Return_True_For_Granted_User()
        {
            (await _permissionManager.IsGrantedForUserAsync(
                "MyPermission1",
                AbpPermissionTestDataBuilder.User1Id
            )).ShouldBeTrue();
        }

        [Fact]
        public async Task Should_Return_True_For_Granted_Current_User()
        {
            _currentUserId = AbpPermissionTestDataBuilder.User1Id;

            (await _permissionManager.IsGrantedForCurrentUserAsync(
                "MyPermission1"
            )).ShouldBeTrue();
        }

        [Fact]
        public async Task Should_Return_False_For_Non_Granted_User()
        {
            (await _permissionManager.IsGrantedForUserAsync(
                "MyPermission1",
                AbpPermissionTestDataBuilder.User2Id
            )).ShouldBeFalse();
        }

        [Fact]
        public async Task Should_Return_False_For_Non_Granted_Current_User()
        {
            _currentUserId = AbpPermissionTestDataBuilder.User2Id;

            (await _permissionManager.IsGrantedForCurrentUserAsync(
                "MyPermission1"
            )).ShouldBeFalse();
        }

        [Fact]
        public async Task Should_Return_False_For_Current_User_If_Anonymous()
        {
            (await _permissionManager.IsGrantedForCurrentUserAsync(
                "MyPermission1"
            )).ShouldBeFalse();
        }
    }
}
