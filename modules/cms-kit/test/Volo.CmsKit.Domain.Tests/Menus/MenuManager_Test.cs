using Shouldly;
using System;
using System.Linq;
using System.Threading.Tasks;
using Volo.CmsKit.Pages;
using Xunit;

namespace Volo.CmsKit.Menus
{
    public class MenuManager_Test : CmsKitDomainTestBase
    {
        private readonly MenuItemManager menuManager;
        private readonly CmsKitTestData testData;
        private readonly IMenuItemRepository menuItemRepository;
        private readonly IPageRepository pageRepository;

        public MenuManager_Test()
        {
            menuManager = GetRequiredService<MenuItemManager>();
            testData = GetRequiredService<CmsKitTestData>();
            menuItemRepository = GetRequiredService<IMenuItemRepository>();
            pageRepository = GetRequiredService<IPageRepository>();
        }

        [Fact]
        public async Task SetPageUrl_ShouldSetUrlSameWithPage_WithExistingPage()
        {
            var page = await pageRepository.GetAsync(testData.Page_1_Id);

            var menuItem = await menuItemRepository.GetAsync(testData.MenuItem_1_Id);

            menuManager.SetPageUrl(menuItem, page);

            menuItem.Url.ShouldNotBeNullOrEmpty();
            menuItem.Url.ShouldBe(PageConsts.UrlPrefix + page.Slug);
        }

        [Fact]
        public async Task MoveAsync_ShouldMoveCorrectly_UnderAnotherMenu()
        {
            await menuManager.MoveAsync(testData.MenuItem_3_Id, testData.MenuItem_1_Id);

            var menu = await menuItemRepository.GetAsync(testData.MenuItem_3_Id);

            menu.ParentId.ShouldBe(testData.MenuItem_1_Id);
        }

        [Fact]
        public async Task MoveAsync_ShouldChangePositionCorrectly_UnderRoot()
        {
            await menuManager.MoveAsync(testData.MenuItem_2_Id, null, 0);

            var menuItems = await menuItemRepository.GetListAsync();

            var menuItem1 = menuItems.First(x => x.Id == testData.MenuItem_1_Id);
            var menuItem2 = menuItems.First(x => x.Id == testData.MenuItem_2_Id);

            menuItem1.Order.ShouldBeGreaterThan(menuItem2.Order);
        }

        [Fact]
        public async Task OrganizeTreeOrderForMenuItem_ShouldWorkProperly_WithNewMenuItem()
        {
            var menu3Id = Guid.NewGuid();

            var menuItems = await menuItemRepository.GetListAsync();

            var menuItem1 = menuItems.First(x => x.Id == testData.MenuItem_1_Id);
            var menuItem2 = menuItems.First(x => x.Id == testData.MenuItem_2_Id);
            var menuItem3 = new MenuItem(menu3Id, "Menu 3", "#");

            menuItems.Add(menuItem3);

            menuItem3.Order = 0;

            menuManager.OrganizeTreeOrderForMenuItem(menuItems, menuItem3);

            menuItem3.Order.ShouldBeLessThan(menuItem1.Order);
            menuItem3.Order.ShouldBeLessThan(menuItem2.Order);
        }
    }
}