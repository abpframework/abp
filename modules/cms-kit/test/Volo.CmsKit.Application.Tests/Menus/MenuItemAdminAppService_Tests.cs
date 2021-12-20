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

namespace Volo.CmsKit.Menus;

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
        var menu = await MenuAdminAppService.GetAsync(TestData.MenuItem_1_Id);

        menu.ShouldNotBeNull();
    }

    public async Task GetListAsync_ShouldWorkProperly()
    {
        var result = await MenuAdminAppService.GetListAsync();

        result.ShouldNotBeNull();
        result.Items.ShouldNotBeEmpty();
        result.Items.Count.ShouldBe(3);
    }

    [Fact]
    public async Task CreateAsync_ShouldWorkProperly_WithOnlyName()
    {
        var name = "My Awesome Menu";
        var menu = await MenuAdminAppService.CreateAsync(new MenuItemCreateInput
        {
            DisplayName = name
        });

        menu.ShouldNotBeNull();
        menu.Id.ShouldNotBe(Guid.Empty);
        menu.DisplayName.ShouldBe(name);
    }

    [Fact]
    public async Task UpdateAsync_ShouldWorkProperly_WithName()
    {
        var newName = "My New Name";
        var newUrl = "my-new-url";
        await WithUnitOfWorkAsync(async () =>
        {
            await MenuAdminAppService.UpdateAsync(TestData.MenuItem_1_Id, new MenuItemUpdateInput
            {
                DisplayName = newName,
                Url = newUrl
            });
        });


        var menu = await MenuRepository.FindAsync(TestData.MenuItem_1_Id);

        menu.ShouldNotBeNull();
        menu.DisplayName.ShouldBe(newName);
        menu.Url.ShouldBe(newUrl);
    }

    [Fact]
    public async Task DeleteAsync_ShouldWorkProperly_WithExistingId()
    {
        await MenuAdminAppService.DeleteAsync(TestData.MenuItem_1_Id);


        var menu = await MenuRepository.FindAsync(TestData.MenuItem_1_Id);

        menu.ShouldBeNull();
    }
}
