using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using Shouldly;
using Volo.Abp.Users;
using Volo.CmsKit.Public.Ratings;
using Xunit;

namespace Volo.CmsKit.Ratings;

public class RatingPublicAppService_Tests : CmsKitApplicationTestBase
{
    private readonly IRatingPublicAppService _ratingAppService;
    private ICurrentUser _currentUser;
    private readonly CmsKitTestData _cmsKitTestData;

    public RatingPublicAppService_Tests()
    {
        _ratingAppService = GetRequiredService<IRatingPublicAppService>();
        _cmsKitTestData = GetRequiredService<CmsKitTestData>();
    }

    protected override void AfterAddApplication(IServiceCollection services)
    {
        _currentUser = Substitute.For<ICurrentUser>();
        services.AddSingleton(_currentUser);
    }

    [Fact]
    public async Task CreateAsync()
    {
        _currentUser.Id.Returns(_cmsKitTestData.User1Id);

        var newRating = await _ratingAppService.CreateAsync(
            _cmsKitTestData.EntityType1,
            _cmsKitTestData.EntityId2,
            new CreateUpdateRatingInput
            {
                StarCount = 4
            });

        UsingDbContext(context =>
        {
            var ratings = context.Set<Rating>().Where(x =>
                x.EntityId == _cmsKitTestData.EntityId2 && x.EntityType == _cmsKitTestData.EntityType1).ToList();

            ratings
                .Any(c => c.Id == newRating.Id && c.CreatorId == newRating.CreatorId &&
                          c.StarCount == newRating.StarCount)
                .ShouldBeTrue();
        });
    }

    [Fact]
    public async Task CreateAsync_Should_Update_If_Rating_Is_Exist()
    {
        _currentUser.Id.Returns(_cmsKitTestData.User1Id);

        var entity =
            (await _ratingAppService.GetGroupedStarCountsAsync(_cmsKitTestData.EntityType1,
                _cmsKitTestData.EntityId1)).FirstOrDefault();

        var updatedEntity = await _ratingAppService.CreateAsync(
            _cmsKitTestData.EntityType1,
            _cmsKitTestData.EntityId1,
            new CreateUpdateRatingInput
            {
                StarCount = 5
            });

        entity.StarCount.ShouldBe(updatedEntity.StarCount);
    }

    [Fact]
    public async Task GetGroupedStarCountsAsync()
    {
        _currentUser.Id.Returns(_cmsKitTestData.User1Id);

        var ratings = await _ratingAppService.GetGroupedStarCountsAsync(
            _cmsKitTestData.EntityType1,
            _cmsKitTestData.EntityId1
        );

        ratings.ShouldNotBeNull();
        ratings.Count.ShouldBeGreaterThan(0);
    }

    [Fact]
    public async Task DeleteAsync()
    {
        _currentUser.Id.Returns(_cmsKitTestData.User1Id);

        var rating = await _ratingAppService.CreateAsync(
            _cmsKitTestData.EntityType1,
            _cmsKitTestData.EntityId1,
            new CreateUpdateRatingInput
            {
                StarCount = 4
            });

        await _ratingAppService.DeleteAsync(_cmsKitTestData.EntityType1, _cmsKitTestData.EntityId1);

        UsingDbContext(context =>
        {
            var deletedComment = context.Set<Rating>().FirstOrDefault(x => x.Id == rating.Id);

            deletedComment.ShouldBeNull();
        });
    }
}
