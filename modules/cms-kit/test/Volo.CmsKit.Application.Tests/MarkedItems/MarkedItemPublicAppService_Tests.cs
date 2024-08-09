using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using Shouldly;
using Volo.Abp.Users;
using Volo.CmsKit.Public.MarkedItems;
using Xunit;

namespace Volo.CmsKit.MarkedItems;

public class MarkedItemPublicAppService_Tests : CmsKitApplicationTestBase
{
    private readonly CmsKitTestData _cmsKitTestData;
    private readonly MarkedItemPublicAppService _markedItemPublicAppService;
    private ICurrentUser _currentUser;
    
    public MarkedItemPublicAppService_Tests()
    {
        _cmsKitTestData = GetRequiredService<CmsKitTestData>();
        _markedItemPublicAppService = GetRequiredService<MarkedItemPublicAppService>();
    }

    protected override void AfterAddApplication(IServiceCollection services)
    {
        _currentUser = Substitute.For<ICurrentUser>();
        services.AddSingleton(_currentUser);
    }

    [Fact]
    public async Task GetForToggleAsync()
    {
        _currentUser.Id.Returns(_cmsKitTestData.User1Id);
        _currentUser.IsAuthenticated.Returns(true);

        var firstMarkedItem = await _markedItemPublicAppService.GetForUserAsync(
            _cmsKitTestData.EntityType1,
            _cmsKitTestData.EntityId1
        );
        var secondMarkedItem = await _markedItemPublicAppService.GetForUserAsync(
            _cmsKitTestData.EntityType2,
            _cmsKitTestData.EntityId1
        );

        firstMarkedItem.IsMarkedByCurrentUser.ShouldBeTrue();
        secondMarkedItem.IsMarkedByCurrentUser.ShouldBeFalse();
    }

    [Fact]
    public async Task ToggleAsync()
    {
        _currentUser.Id.Returns(_cmsKitTestData.User1Id);

        var firstToggleResult = await _markedItemPublicAppService.ToggleAsync(
            _cmsKitTestData.EntityType1,
            _cmsKitTestData.EntityId1
        );
        var secondToggleResult = await _markedItemPublicAppService.ToggleAsync(
            _cmsKitTestData.EntityType2,
            _cmsKitTestData.EntityId1
        );

        firstToggleResult.ShouldBeFalse();
        secondToggleResult.ShouldBeTrue();
    }
}
