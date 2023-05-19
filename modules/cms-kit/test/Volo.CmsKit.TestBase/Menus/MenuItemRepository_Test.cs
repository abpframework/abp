using System;
using System.Threading.Tasks;
using Shouldly;
using Volo.Abp.Modularity;

namespace Volo.CmsKit.Menus;

public abstract class MenuItemRepository_Test<TStartupModule> : CmsKitTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{

    private readonly CmsKitTestData testData;
    private readonly IMenuItemRepository menuItemRepository;

    public MenuItemRepository_Test()
    {
        testData = GetRequiredService<CmsKitTestData>();
        menuItemRepository = GetRequiredService<IMenuItemRepository>();
    }
}
