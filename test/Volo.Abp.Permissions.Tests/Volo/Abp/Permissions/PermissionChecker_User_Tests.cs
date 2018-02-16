using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using Shouldly;
using Volo.Abp.Session;
using Xunit;

namespace Volo.Abp.Permissions
{
    public class PermissionChecker_User_Tests : PermissionTestBase
    {
        private readonly IPermissionChecker _permissionChecker;

        private Guid? _currentUserId;

        public PermissionChecker_User_Tests()
        {
            _permissionChecker = GetRequiredService<IPermissionChecker>();
        }

        protected override void AfterAddApplication(IServiceCollection services)
        {
            var currentUser = Substitute.For<ICurrentUser>();
            currentUser.Id.Returns(ci => _currentUserId);
            services.AddSingleton(currentUser);
        }

        [Fact]
        public async Task Should_Return_True_For_Granted_Current_User()
        {
            _currentUserId = PermissionTestDataBuilder.User1Id;

            (await _permissionChecker.IsGrantedAsync(
                "MyPermission1"
            )).ShouldBeTrue();
        }

        [Fact]
        public async Task Should_Return_False_For_Non_Granted_Current_User()
        {
            _currentUserId = PermissionTestDataBuilder.User2Id;

            (await _permissionChecker.IsGrantedAsync(
                "MyPermission1"
            )).ShouldBeFalse();
        }

        [Fact]
        public async Task Should_Return_False_For_Current_User_If_Anonymous()
        {
            (await _permissionChecker.IsGrantedAsync(
                "MyPermission1"
            )).ShouldBeFalse();
        }
    }
}
