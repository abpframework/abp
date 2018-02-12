using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using Shouldly;
using Volo.Abp.Session;
using Xunit;

namespace Volo.Abp.Permissions
{
    public class PermissionManager_User_Tests : PermissionTestBase
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
                PermissionTestDataBuilder.User1Id
            )).ShouldBeTrue();
        }

        [Fact]
        public async Task Should_Return_True_For_Granted_Current_User()
        {
            _currentUserId = PermissionTestDataBuilder.User1Id;

            (await _permissionManager.IsGrantedForCurrentUserAsync(
                "MyPermission1"
            )).ShouldBeTrue();
        }

        [Fact]
        public async Task Should_Return_False_For_Non_Granted_User()
        {
            (await _permissionManager.IsGrantedForUserAsync(
                "MyPermission1",
                PermissionTestDataBuilder.User2Id
            )).ShouldBeFalse();
        }

        [Fact]
        public async Task Should_Return_False_For_Non_Granted_Current_User()
        {
            _currentUserId = PermissionTestDataBuilder.User2Id;

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

        [Fact]
        public async Task Should_Get_List_Of_Granted_Permissions_For_A_User()
        {
            //User1
            var permission = await _permissionManager.GetAllForUserAsync(PermissionTestDataBuilder.User1Id);
            permission.Count.ShouldBeGreaterThan(0);
            permission.ShouldContain(p => p.Name == "MyPermission1" && p.IsGranted && p.ProviderName == UserPermissionValueProvider.ProviderName);

            //User2
            permission = await _permissionManager.GetAllForUserAsync(PermissionTestDataBuilder.User2Id);
            permission.Count.ShouldBeGreaterThan(0);
            permission.ShouldContain(p => p.Name == "MyPermission1" && !p.IsGranted);
        }

        [Fact]
        public async Task Should_Get_List_Of_Granted_Permissions_For_Current_User()
        {
            //User1
            _currentUserId = PermissionTestDataBuilder.User1Id;
            var permission = await _permissionManager.GetAllForCurrentUserAsync();
            permission.Count.ShouldBeGreaterThan(0);
            permission.ShouldContain(p => p.Name == "MyPermission1" && p.IsGranted && p.ProviderName == UserPermissionValueProvider.ProviderName);

            //User2
            _currentUserId = PermissionTestDataBuilder.User2Id;
            permission = await _permissionManager.GetAllForCurrentUserAsync();
            permission.Count.ShouldBeGreaterThan(0);
            permission.ShouldContain(p => p.Name == "MyPermission1" && !p.IsGranted);
        }

        [Fact]
        public async Task Should_Grant_Permission_For_A_User()
        {
            (await _permissionManager.IsGrantedForUserAsync(
                "MyPermission1",
                PermissionTestDataBuilder.User2Id
            )).ShouldBeFalse();

            await _permissionManager.SetForUserAsync(PermissionTestDataBuilder.User2Id, "MyPermission1", true);

            (await _permissionManager.IsGrantedForUserAsync(
                "MyPermission1",
                PermissionTestDataBuilder.User2Id
            )).ShouldBeTrue();
        }
    }
}
