using System.Linq;
using System.Threading.Tasks;
using Shouldly;
using Volo.Abp.Modularity;
using Volo.Abp.Settings;
using Xunit;

namespace Volo.Abp.SettingManagement
{
    public abstract class SettingRepository_Tests<TStartupModule> : SettingManagementTestBase<TStartupModule>
        where TStartupModule : IAbpModule
    {
        protected ISettingRepository SettingRepository { get; }
        protected SettingTestData TestData { get; }

        protected SettingRepository_Tests()
        {
            SettingRepository = GetRequiredService<ISettingRepository>();
            TestData = GetRequiredService<SettingTestData>();
        }

        [Fact]
        public async Task FindAsync()
        {
            (await SettingRepository.FindAsync(
                "MySetting1",
                GlobalSettingValueProvider.ProviderName,
                null
            )).Value.ShouldBe("42");

            (await SettingRepository.FindAsync(
                "MySetting2",
                UserSettingValueProvider.ProviderName,
                TestData.User1Id.ToString()
            )).Value.ShouldBe("user1-store-value");

            (await SettingRepository.FindAsync(
                "Undefined-Setting",
                GlobalSettingValueProvider.ProviderName,
                null
            )).ShouldBeNull();
        }

        [Fact]
        public async Task GetListAsync()
        {
            var settings = await SettingRepository.GetListAsync(GlobalSettingValueProvider.ProviderName, null);
            settings.Any().ShouldBeTrue();
            settings.ShouldContain(s => s.Name == "MySetting1" && s.Value == "42");
            settings.ShouldContain(s => s.Name == "MySetting2" && s.Value == "default-store-value");
        }
    }
}
