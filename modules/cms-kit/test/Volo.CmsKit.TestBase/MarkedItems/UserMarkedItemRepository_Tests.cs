using System.Linq;
using System.Threading.Tasks;
using Shouldly;
using Volo.Abp.Modularity;
using Xunit;

namespace Volo.CmsKit.MarkedItems;
public abstract class UserMarkedItemRepository_Tests<TStartupModule> : CmsKitTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{
    private readonly CmsKitTestData _cmsKitTestData;
    private readonly IUserMarkedItemRepository _userMarkedItemRepository;

    protected UserMarkedItemRepository_Tests()
    {
        _cmsKitTestData = GetRequiredService<CmsKitTestData>();
        _userMarkedItemRepository = GetRequiredService<IUserMarkedItemRepository>();
    }

    [Fact]
    public async Task FindAsync()
    {
        var markedItem = await _userMarkedItemRepository.FindAsync(
            _cmsKitTestData.User1Id,
            _cmsKitTestData.EntityType1,
            _cmsKitTestData.EntityId1
        );

        markedItem.ShouldNotBeNull();
        markedItem.CreatorId.ShouldBe(_cmsKitTestData.User1Id);
        markedItem.EntityId.ShouldBe(_cmsKitTestData.EntityId1);
        markedItem.EntityType.ShouldBe(_cmsKitTestData.EntityType1);
    }

    [Fact]
    public async Task GetListForUserAsync()
    {
        var markedItems = await _userMarkedItemRepository.GetListForUserAsync(
            _cmsKitTestData.User1Id,
            _cmsKitTestData.EntityType1
        );

        markedItems.Count.ShouldBe(2);
    }


    [Fact]
    public async Task GetEntityIdsFilteredByUserAsync()
    {
        var entityIds = await _userMarkedItemRepository.GetEntityIdsFilteredByUserAsync(_cmsKitTestData.User1Id, _cmsKitTestData.EntityType1);

        entityIds.ShouldNotBeNull();
        entityIds.ShouldNotBeEmpty();

        entityIds.Count.ShouldBe(2);
    }
}
