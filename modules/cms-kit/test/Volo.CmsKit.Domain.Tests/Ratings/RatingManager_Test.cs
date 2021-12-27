using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.CmsKit.Blogs;
using Volo.CmsKit.Users;
using Xunit;

namespace Volo.CmsKit.Ratings;

public class RatingManager_Test : CmsKitDomainTestBase
{
    private readonly CmsKitTestData _cmsKitTestData;
    private readonly RatingManager _ratingManager;
    private readonly ICmsUserRepository _userRepository;

    public RatingManager_Test()
    {
        _cmsKitTestData = GetRequiredService<CmsKitTestData>();
        _ratingManager = GetRequiredService<RatingManager>();
        _userRepository = GetRequiredService<ICmsUserRepository>();
    }

    [Fact]
    public async Task SetStarAsync_ShouldCreate_WhenFirstCall()
    {
        var user = await _userRepository.GetAsync(_cmsKitTestData.User1Id);
        short starCount = 4;

        var rating = await _ratingManager.SetStarAsync(user, _cmsKitTestData.EntityType1, _cmsKitTestData.BlogPost_1_Id.ToString(), starCount);

        rating.ShouldNotBeNull();
        rating.Id.ShouldNotBe(Guid.Empty);
        rating.StarCount.ShouldBe(starCount);
    }

    [Fact]
    public async Task SetStarAsync_ShouldUpdate_WithExistingRating()
    {
        var user = await _userRepository.GetAsync(_cmsKitTestData.User1Id);
        short starCount = 2;

        var rating = await _ratingManager.SetStarAsync(user, _cmsKitTestData.EntityType1, _cmsKitTestData.EntityId1, starCount);

        rating.ShouldNotBeNull();
        rating.Id.ShouldNotBe(Guid.Empty);
        rating.StarCount.ShouldBe(starCount);
    }

    [Fact]
    public async Task SetStarAsync_ShouldThrowException_WithNotConfiguredentityType()
    {
        var user = await _userRepository.GetAsync(_cmsKitTestData.User1Id);
        var notConfiguredEntityType = "AnyOtherEntityType";
        short starCount = 3;

        var exception = await Should.ThrowAsync<EntityCantHaveRatingException>(async () =>
                            await _ratingManager.SetStarAsync(user, notConfiguredEntityType, "1", starCount));

        exception.ShouldNotBeNull();
        exception.EntityType.ShouldBe(notConfiguredEntityType);
    }
}
