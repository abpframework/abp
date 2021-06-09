using System.Threading.Tasks;
using Shouldly;
using Volo.Abp.Modularity;
using Xunit;

namespace Volo.CmsKit.Menus
{
    public abstract class MenuRepository_Test<TStartupModule> : CmsKitTestBase<TStartupModule>
        where TStartupModule : IAbpModule
    {
        
        private readonly CmsKitTestData testData;
        private readonly IMenuRepository menuRepository;

        public MenuRepository_Test()
        {
            testData = GetRequiredService<CmsKitTestData>();
            menuRepository = GetRequiredService<IMenuRepository>();
        }

        [Fact]
        public async Task GetMainMenuAsync_ShouldWorkProperly()
        {
            var mainMenu = await menuRepository.FindMainMenuAsync();

            mainMenu.ShouldNotBeNull();
            mainMenu.Id.ShouldBe(testData.Menu_1_Id);
        }

        [Fact]
        public async Task GetMainMenuAsync_ShouldWorkProperly_WithIncludeDetails()
        {
            var mainMenu = await menuRepository.FindMainMenuAsync(includeDetails: true);

            mainMenu.ShouldNotBeNull();
            mainMenu.Id.ShouldBe(testData.Menu_1_Id);
            mainMenu.Items.ShouldNotBeEmpty();
            mainMenu.Items.Count.ShouldBe(2);
        }
    }
}