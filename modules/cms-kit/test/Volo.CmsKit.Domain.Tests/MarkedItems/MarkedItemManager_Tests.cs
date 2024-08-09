using System.Threading.Tasks;
using Shouldly;
using Xunit;

namespace Volo.CmsKit.MarkedItems;

public class MarkedItemManager_Tests : CmsKitDomainTestBase
{
    private readonly CmsKitTestData _cmsKitTestData;
    private readonly MarkedItemManager _markedItemManager;

    public MarkedItemManager_Tests()
    {
        _cmsKitTestData = GetRequiredService<CmsKitTestData>();
        _markedItemManager = GetRequiredService<MarkedItemManager>();
    }

    [Fact]
    public async Task ToggleAsync()
    {
        var firstToggleResult = await _markedItemManager.ToggleUserMarkedItemAsync(
            _cmsKitTestData.User1Id,
            _cmsKitTestData.EntityType2,
            _cmsKitTestData.EntityId1
        );

        var secondToggleResult = await _markedItemManager.ToggleUserMarkedItemAsync(
             _cmsKitTestData.User1Id,
            _cmsKitTestData.EntityType2,
            _cmsKitTestData.EntityId1
        );

        firstToggleResult.ShouldBeTrue();
        secondToggleResult.ShouldBeFalse();
    }
}
