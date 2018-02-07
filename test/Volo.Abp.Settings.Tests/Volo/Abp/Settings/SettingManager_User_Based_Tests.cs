using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using Shouldly;
using Volo.Abp.Session;
using Xunit;

namespace Volo.Abp.Settings
{
    public class SettingManager_User_Based_Tests: AbpSettingsTestBase
    {
        private Guid _currentUserId;
        private readonly ISettingManager _settingManager;

        public SettingManager_User_Based_Tests()
        {
            _settingManager = GetRequiredService<ISettingManager>();
        }

        protected override void AfterAddApplication(IServiceCollection services)
        {
            ICurrentUser currentUser = Substitute.For<ICurrentUser>();
            currentUser.Id.Returns(ci => _currentUserId);
            services.AddSingleton(currentUser);
        }

        [Fact]
        public async Task Should_Get_From_Store_For_Given_User()
        {
            (await _settingManager.GetOrNullForUserAsync("MySetting2", AbpIdentityTestDataBuilder.User1Id)).ShouldBe("user1-store-value");
            (await _settingManager.GetOrNullForUserAsync("MySetting2", AbpIdentityTestDataBuilder.User2Id)).ShouldBe("user2-store-value");
        }

        [Fact]
        public async Task Should_Fallback_To_Default_Store_Value_When_No_Value_For_Given_User()
        {
            (await _settingManager.GetOrNullForUserAsync("MySetting2", Guid.NewGuid())).ShouldBe("default-store-value");
        }

        [Fact]
        public async Task Should_Not_Fallback_To_Default_Store_Value_When_No_Value_For_Given_User_But_Specified_Fallback_As_False()
        {
            (await _settingManager.GetOrNullForUserAsync("MySetting2", Guid.NewGuid(), fallback: false)).ShouldBeNull();
        }

        [Fact]
        public async Task Should_Get_From_Store_For_Current_User()
        {
            _currentUserId = AbpIdentityTestDataBuilder.User1Id;
            (await _settingManager.GetOrNullAsync("MySetting2")).ShouldBe("user1-store-value");

            _currentUserId = AbpIdentityTestDataBuilder.User2Id;
            (await _settingManager.GetOrNullAsync("MySetting2")).ShouldBe("user2-store-value");
        }

        [Fact]
        public async Task Should_Fallback_To_Default_Store_Value_When_No_Value_For_Current_User()
        {
            _currentUserId = Guid.NewGuid();
            (await _settingManager.GetOrNullAsync("MySetting2")).ShouldBe("default-store-value");
        }

        [Fact]
        public async Task Should_Get_From_Store_For_Current_User_With_GetOrNullForCurrentUserAsync()
        {
            _currentUserId = AbpIdentityTestDataBuilder.User1Id;
            (await _settingManager.GetOrNullForCurrentUserAsync("MySetting2")).ShouldBe("user1-store-value");

            _currentUserId = AbpIdentityTestDataBuilder.User2Id;
            (await _settingManager.GetOrNullForCurrentUserAsync("MySetting2")).ShouldBe("user2-store-value");
        }

        [Fact]
        public async Task Should_Fallback_To_Default_Store_Value_When_No_Value_For_Current_User_With_GetOrNullForCurrentUserAsync()
        {
            _currentUserId = Guid.NewGuid();
            (await _settingManager.GetOrNullAsync("MySetting2")).ShouldBe("default-store-value");
        }

        [Fact]
        public async Task Should_Not_Fallback_To_Default_Store_Value_When_No_Value_For_Current_User_But_Specified_Fallback_As_False()
        {
            _currentUserId = Guid.NewGuid();
            (await _settingManager.GetOrNullForCurrentUserAsync("MySetting2", fallback: false)).ShouldBeNull();
        }
    }
}