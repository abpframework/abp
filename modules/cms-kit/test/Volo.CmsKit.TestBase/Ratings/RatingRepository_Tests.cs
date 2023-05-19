using System.Threading.Tasks;
using Shouldly;
using Volo.Abp.Modularity;
using Xunit;

namespace Volo.CmsKit.Ratings;

public abstract class RatingRepository_Tests<TStartupModule> : CmsKitTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{
    private readonly CmsKitTestData _cmsKitTestData;
    private readonly IRatingRepository _ratingRepository;

    public RatingRepository_Tests()
    {
        _cmsKitTestData = GetRequiredService<CmsKitTestData>();
        _ratingRepository = GetRequiredService<IRatingRepository>();
    }

    [Fact]
    public async Task GetCurrentUserRatingAsync()
    {
        var userRating = await _ratingRepository.GetCurrentUserRatingAsync(_cmsKitTestData.EntityType1,
            _cmsKitTestData.EntityId1, _cmsKitTestData.User1Id);

        userRating.ShouldNotBeNull();
        userRating.EntityId.ShouldBe(_cmsKitTestData.EntityId1);
        userRating.EntityType.ShouldBe(_cmsKitTestData.EntityType1);
        userRating.CreatorId.ShouldBe(_cmsKitTestData.User1Id);
    }

    [Fact]
    public async Task GetGroupedStarCountsAsync()
    {
        var list = await _ratingRepository.GetGroupedStarCountsAsync(_cmsKitTestData.EntityType1,
            _cmsKitTestData.EntityId1);

        list.ShouldNotBeNull();
    }
}
