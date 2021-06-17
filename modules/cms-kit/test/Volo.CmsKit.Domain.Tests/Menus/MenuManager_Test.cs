using System;
using System.Linq;
using System.Threading.Tasks;
using Nito.AsyncEx;
using Shouldly;
using Volo.CmsKit.Pages;
using Xunit;

namespace Volo.CmsKit.Menus
{
    public class MenuManager_Test : CmsKitDomainTestBase
    {
        private readonly MenuManager menuManager;
        private readonly CmsKitTestData testData;
        private readonly IMenuRepository menuRepository;
        private readonly IPageRepository pageRepository;

        public MenuManager_Test()
        {
            menuManager = GetRequiredService<MenuManager>();
            testData = GetRequiredService<CmsKitTestData>();
            menuRepository = GetRequiredService<IMenuRepository>();
            pageRepository = GetRequiredService<IPageRepository>();
        }

        [Fact]
        public async Task SetPageUrl_ShouldSetUrlSameWithPage_WithExistingPage()
        {
            var page = await pageRepository.GetAsync(testData.Page_1_Id);
            var menu = await menuRepository.GetAsync(testData.Menu_1_Id);
            var menuItem = menu.Items.First();

            menuManager.SetPageUrl(menuItem, page);

            menuItem.Url.ShouldNotBeNullOrEmpty();
            menuItem.Url.ShouldBe(PageConsts.UrlPrefix + page.Slug);
        }

        [Fact]
        public async Task MoveAsync_ShouldMoveCorrectly_UnderAnotherMenu()
        {
            await menuManager.MoveAsync(testData.Menu_1_Id, testData.MenuItem_2_Id, testData.MenuItem_1_Id);

            var menu = await menuRepository.GetAsync(testData.Menu_1_Id);

            menu.Items.ShouldContain(
                x => x.ParentId == testData.MenuItem_1_Id
                     && x.Id == testData.MenuItem_2_Id);
        }

        [Fact]
        public async Task MoveAsync_ShouldChangePositionCorrectly_UnderSameParent()
        {
            await menuManager.MoveAsync(testData.Menu_1_Id, testData.MenuItem_2_Id, null, 0);

            var menu = await menuRepository.GetAsync(testData.Menu_1_Id);

            var menuItem1 = menu.Items.First(x => x.Id == testData.MenuItem_1_Id);
            var menuItem2 = menu.Items.First(x => x.Id == testData.MenuItem_2_Id);

            menuItem1.Order.ShouldBeGreaterThan(menuItem2.Order);
        }

        [Fact]
        public async Task OrganizeTreeOrderForMenuItem_ShouldWorkProperly_WithNewMenuItem()
        {
            var menu3Id = Guid.NewGuid();
            var menu = await menuRepository.GetAsync(testData.Menu_1_Id);
            var menuItem1 = menu.Items.First(x => x.Id == testData.MenuItem_1_Id);
            var menuItem2 = menu.Items.First(x => x.Id == testData.MenuItem_2_Id);
            var menuItem3 = new MenuItem(menu3Id, menu.Id, "Menu 3", "#");

            menu.Items.Add(menuItem3);

            menuItem3.Order = 0;

            menuManager.OrganizeTreeOrderForMenuItem(menu, menuItem3);

            menuItem3.Order.ShouldBeLessThan(menuItem1.Order);
            menuItem3.Order.ShouldBeLessThan(menuItem2.Order);
        }

        [Fact]
        public async Task SetMainMenuAsync_ShouldSetOnlyOneMainMenu()
        {
            await menuManager.SetMainMenuAsync(testData.Menu_2_Id);

            var menuList = await menuRepository.GetListAsync();

            var isMainMenuTrueCount = menuList.Count(menu => menu.IsMainMenu);

            isMainMenuTrueCount.ShouldBe(1);
            menuList.ShouldContain(menu => menu.IsMainMenu && menu.Id == testData.Menu_2_Id);
        }

        [Fact]
        public async Task UnSetMainMenuAsync_ShouldUnsetProperly()
        {
            await menuManager.UnSetMainMenuAsync(testData.Menu_1_Id);

            var menuList = await menuRepository.GetListAsync();

            var isMainMenuTrueCount = menuList.Count(menu => menu.IsMainMenu);
            
            isMainMenuTrueCount.ShouldBe(0);
            menuList.ShouldNotContain(menu => menu.IsMainMenu && menu.Id == testData.Menu_1_Id);
        }
    }
}