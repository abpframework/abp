using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities;
using Volo.CmsKit.Admin.Menus;
using Xunit;

namespace Volo.CmsKit.Menus
{
    public class MenuItemAdminAppService_Tests : CmsKitApplicationTestBase
    {
        protected IMenuItemAdminAppService MenuAdminAppService { get; }
        protected CmsKitTestData TestData { get; }
        protected IMenuItemRepository MenuRepository { get; }

        public MenuItemAdminAppService_Tests()
        {
            MenuAdminAppService = GetRequiredService<IMenuItemAdminAppService>();
            TestData = GetRequiredService<CmsKitTestData>();
            MenuRepository = GetRequiredService<IMenuItemRepository>();
        }

        [Fact]
        public async Task GetAsync_ShouldWorkProperly_WithCorrectId()
        {
            var menu = await MenuAdminAppService.GetAsync(TestData.Menu_1_Id);

            menu.ShouldNotBeNull();
            menu.Name.ShouldBe(TestData.Menu_1_Name);
        }

        [Fact]
        public async Task GetAsync_ShouldGetItemsProperly_WithCorrectId()
        {
            var menu = await MenuAdminAppService.GetAsync(TestData.Menu_1_Id);

            menu.ShouldNotBeNull();
            menu.Items.ShouldNotBeEmpty();
            menu.Items.Count.ShouldBe(2);
        }

        [Fact]
        public async Task CreateAsync_ShouldWorkProperly_WithOnlyName()
        {
            var name = "My Awesome Menu";
            var menu = await MenuAdminAppService.CreateAsync(new MenuCreateInput
            {
                Name = name
            });

            menu.ShouldNotBeNull();
            menu.Id.ShouldNotBe(Guid.Empty);
            menu.Name.ShouldBe(name);
        }

        [Fact]
        public async Task UpdateAsync_ShouldWorkProperly_WithName()
        {
            var newName = "My New Name";

            await MenuAdminAppService.UpdateAsync(TestData.Menu_1_Id, new MenuUpdateInput
            {
                Name = newName
            });

            var menu = await MenuRepository.FindAsync(TestData.Menu_1_Id);

            menu.ShouldNotBeNull();
            menu.Name.ShouldBe(newName);
        }

        [Fact]
        public async Task DeleteAsync_ShouldWorkProperly_WithExistingId()
        {
            await MenuAdminAppService.DeleteAsync(TestData.Menu_1_Id);


            var menu = await MenuRepository.FindAsync(TestData.Menu_1_Id);

            menu.ShouldBeNull();
        }
    }
}
