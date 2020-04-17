using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using Shouldly;
using System;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Settings;
using Volo.Abp.Users;
using Xunit;

namespace Volo.Abp.SettingManagement
{
    public class SettingManager_User_Tests : SettingsTestBase
    {
        private Guid? _currentUserId;

        private readonly ISettingManager _settingManager;
        private readonly ISettingProvider _settingProvider;
        private readonly SettingTestData _testData;

        public SettingManager_User_Tests()
        {
            _settingManager = GetRequiredService<ISettingManager>();
            _settingProvider = GetRequiredService<ISettingProvider>();
            _testData = GetRequiredService<SettingTestData>();
        }

        protected override void AfterAddApplication(IServiceCollection services)
        {
            var currentUser = Substitute.For<ICurrentUser>();
            currentUser.Id.Returns(ci => _currentUserId);
            services.AddSingleton(currentUser);
        }

        [Fact]
        public async Task Should_Get_From_Store_For_Given_User()
        {
            (await _settingManager.GetOrNullForUserAsync("MySetting2", _testData.User1Id)).ShouldBe("user1-store-value");
            (await _settingManager.GetOrNullForUserAsync("MySetting2", _testData.User2Id)).ShouldBe("user2-store-value");
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
            _currentUserId = _testData.User1Id;
            (await _settingProvider.GetOrNullAsync("MySetting2")).ShouldBe("user1-store-value");

            _currentUserId = _testData.User2Id;
            (await _settingProvider.GetOrNullAsync("MySetting2")).ShouldBe("user2-store-value");
        }

        [Fact]
        public async Task Should_Fallback_To_Default_Store_Value_When_No_Value_For_Current_User()
        {
            _currentUserId = Guid.NewGuid();
            (await _settingProvider.GetOrNullAsync("MySetting2")).ShouldBe("default-store-value");
        }

        [Fact]
        public async Task Should_Get_From_Store_For_Current_User_With_GetOrNullForCurrentUserAsync()
        {
            _currentUserId = _testData.User1Id;
            (await _settingManager.GetOrNullForCurrentUserAsync("MySetting2")).ShouldBe("user1-store-value");

            _currentUserId = _testData.User2Id;
            (await _settingManager.GetOrNullForCurrentUserAsync("MySetting2")).ShouldBe("user2-store-value");
        }

        [Fact]
        public async Task Should_Fallback_To_Default_Store_Value_When_No_Value_For_Current_User_With_GetOrNullForCurrentUserAsync()
        {
            _currentUserId = Guid.NewGuid();
            (await _settingProvider.GetOrNullAsync("MySetting2")).ShouldBe("default-store-value");
        }

        [Fact]
        public async Task Should_Not_Fallback_To_Default_Store_Value_When_No_Value_For_Current_User_But_Specified_Fallback_As_False()
        {
            _currentUserId = Guid.NewGuid();
            (await _settingManager.GetOrNullForCurrentUserAsync("MySetting2", fallback: false)).ShouldBeNull();
        }

        [Fact]
        public async Task Should_Get_All_From_Store_For_Given_User()
        {
            var settingValues = await _settingManager.GetAllForUserAsync(_testData.User1Id);
            settingValues.ShouldContain(sv => sv.Name == "MySetting1" && sv.Value == "42");
            settingValues.ShouldContain(sv => sv.Name == "MySetting2" && sv.Value == "user1-store-value");
            settingValues.ShouldContain(sv => sv.Name == "SettingNotSetInStore" && sv.Value == "default-value");
        }

        [Fact]
        public async Task Should_Get_All_From_Store_For_Given_User_Without_Fallback()
        {
            var settingValues = await _settingManager.GetAllForUserAsync(_testData.User1Id, fallback: false);
            settingValues.ShouldContain(sv => sv.Name == "MySetting2" && sv.Value == "user1-store-value");
            settingValues.ShouldContain(sv => sv.Name == "MySettingWithoutInherit" && sv.Value == "user1-store-value");
            settingValues.ShouldNotContain(sv => sv.Name == "MySetting1");
        }

        [Fact]
        public async Task Should_Delete_Setting_Record_When_Set_To_Null()
        {
            await _settingManager.SetForUserAsync(_testData.User1Id, "MySetting2", null);

            GetSettingsFromDbContext(
                UserSettingValueProvider.ProviderName,
                _testData.User1Id.ToString(),
                "MySetting2"
            ).Count.ShouldBe(0);
        }

        [Fact]
        public async Task Should_Change_User_Setting()
        {
            (await _settingManager.GetOrNullForUserAsync("MySetting2", _testData.User1Id))
                .ShouldBe("user1-store-value");

            await _settingManager.SetForUserAsync(_testData.User1Id, "MySetting2", "user1-new-store-value");

            (await _settingManager.GetOrNullForUserAsync("MySetting2", _testData.User1Id))
                .ShouldBe("user1-new-store-value");

            GetSettingsFromDbContext(
                UserSettingValueProvider.ProviderName,
                _testData.User1Id.ToString(),
                "MySetting2"
            ).Single().Value.ShouldBe("user1-new-store-value");
        }

        [Fact]
        public async Task Should_Delete_Setting_Record_When_Set_To_Fallback_Value()
        {
            await _settingManager.SetForUserAsync(
                _testData.User1Id,
                "MySetting2",
                "default-store-value"
            );

            GetSettingsFromDbContext(
                UserSettingValueProvider.ProviderName,
                _testData.User1Id.ToString(),
                "MySetting2"
            ).Count.ShouldBe(0);

            (await _settingManager.GetOrNullForUserAsync("MySetting2", _testData.User1Id))
                .ShouldBe("default-store-value");
        }

        [Fact]
        public async Task Should_Not_Delete_Setting_Record_When_Set_To_Fallback_Value_If_Forced()
        {
            await _settingManager.SetForUserAsync(
                _testData.User1Id,
                "MySetting2",
                "default-store-value",
                forceToSet: true
            );

            GetSettingsFromDbContext(
                UserSettingValueProvider.ProviderName,
                _testData.User1Id.ToString(),
                "MySetting2"
            ).Single().Value.ShouldBe("default-store-value");

            (await _settingManager.GetOrNullForUserAsync("MySetting2", _testData.User1Id))
                .ShouldBe("default-store-value");
        }

        [Fact]
        public async Task Should_Get_For_Given_User_For_Non_Inherited_Setting()
        {
            (await _settingManager.GetOrNullForUserAsync("MySettingWithoutInherit", _testData.User1Id)).ShouldBe("user1-store-value");
            (await _settingManager.GetOrNullForUserAsync("MySettingWithoutInherit", _testData.User2Id)).ShouldBeNull(); //Does not inherit!
            (await _settingManager.GetOrNullGlobalAsync("MySettingWithoutInherit")).ShouldBe("default-store-value");
        }
    }
}