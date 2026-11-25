using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using Shouldly;
using Volo.Abp.Authorization;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Users;
using Volo.Abp.MultiTenancy;
using Volo.CmsKit.GlobalFeatures;
using Volo.CmsKit.Public.Comments;
using Volo.CmsKit.Public.MarkedItems;
using Volo.CmsKit.Public.Ratings;
using Volo.CmsKit.Public.Reactions;
using Volo.CmsKit.Reactions;
using Xunit;

namespace Volo.CmsKit.Blogs;

public class BlogInteractionFeature_Tests : CmsKitApplicationTestBase
{
    private ICurrentUser _user;

    public BlogInteractionFeature_Tests()
    {
        _user.Id.Returns(GetRequiredService<CmsKitTestData>().User1Id);
        _user.IsAuthenticated.Returns(true);
    }

    protected override void AfterAddApplication(IServiceCollection services)
    {
        base.AfterAddApplication(services);
        _user = Substitute.For<ICurrentUser>();
        services.AddSingleton(_user);
    }

    [Theory]
    [InlineData(CommentsFeature.Name)]
    [InlineData(RatingsFeature.Name)]
    [InlineData(ReactionsFeature.Name)]
    public async Task Disabled_Blog_Features_Should_Reject_Public_Writes_And_Allow_Them_After_Reenabling(string feature)
    {
        var data = GetRequiredService<CmsKitTestData>();
        await SetFeatureAsync(feature, false);
        await Should.ThrowAsync<AbpAuthorizationException>(() => WriteAsync(feature, BlogPostConsts.EntityType, data.BlogPost_1_Id.ToString()));
        await AssertNoInteractionAsync(data.BlogPost_1_Id.ToString());

        await SetFeatureAsync(feature, true);
        await Should.NotThrowAsync(() => WriteAsync(feature, BlogPostConsts.EntityType, data.BlogPost_1_Id.ToString()));
    }

    [Theory]
    [InlineData(CommentsFeature.Name)]
    [InlineData(RatingsFeature.Name)]
    [InlineData(ReactionsFeature.Name)]
    public async Task Unset_Blog_Features_Should_Remain_Enabled(string feature)
    {
        var data = GetRequiredService<CmsKitTestData>();
        (await GetRequiredService<IBlogFeatureRepository>().FindAsync(data.Blog_Id, feature)).ShouldBeNull();
        await Should.NotThrowAsync(() => WriteAsync(feature, BlogPostConsts.EntityType, data.BlogPost_1_Id.ToString()));
    }

    [Theory]
    [InlineData(CommentsFeature.Name)]
    [InlineData(RatingsFeature.Name)]
    [InlineData(ReactionsFeature.Name)]
    public async Task Other_Entity_Types_Should_Not_Be_Restricted_By_A_Blog_Feature(string feature)
    {
        var data = GetRequiredService<CmsKitTestData>();
        await SetFeatureAsync(feature, false);
        await Should.NotThrowAsync(() => WriteAsync(feature, data.EntityType1, data.EntityId1));
    }

    [Theory]
    [InlineData(CommentsFeature.Name)]
    [InlineData(RatingsFeature.Name)]
    [InlineData(ReactionsFeature.Name)]
    public async Task Missing_Or_Invalid_Blog_Posts_Should_Not_Accept_Writes(string feature)
    {
        foreach (var id in new[] { Guid.NewGuid().ToString(), "not-a-post-id" })
        {
            await Should.ThrowAsync<EntityNotFoundException>(() => WriteAsync(feature, BlogPostConsts.EntityType, id));
            await AssertNoInteractionAsync(id);
        }
    }

    [Theory]
    [InlineData("blogpost")]
    [InlineData("Blog\u00adPost")]
    public async Task Entity_Type_Comparison_Should_Match_The_Definition_Stores(string entityType)
    {
        var data = GetRequiredService<CmsKitTestData>();
        await SetFeatureAsync(ReactionsFeature.Name, false);
        await Should.ThrowAsync<AbpAuthorizationException>(() => WriteAsync(ReactionsFeature.Name, entityType, data.BlogPost_1_Id.ToString()));
    }

    private Task SetFeatureAsync(string feature, bool enabled)
    {
        return WithUnitOfWorkAsync(() => GetRequiredService<BlogFeatureManager>()
            .SetAsync(GetRequiredService<CmsKitTestData>().Blog_Id, feature, enabled));
    }

    private async Task WriteAsync(string feature, string entityType, string entityId)
    {
        switch (feature)
        {
            case CommentsFeature.Name:
                await GetRequiredService<ICommentPublicAppService>().CreateAsync(entityType, entityId,
                    new CreateCommentInput { Text = "Feature boundary", IdempotencyToken = Guid.NewGuid().ToString("N") });
                break;
            case RatingsFeature.Name:
                await GetRequiredService<IRatingPublicAppService>().CreateAsync(entityType, entityId,
                    new CreateUpdateRatingInput { StarCount = 4 });
                break;
            case ReactionsFeature.Name:
                await GetRequiredService<IReactionPublicAppService>().CreateAsync(entityType, entityId, StandardReactions.ThumbsUp);
                break;
        }
    }

    private async Task AssertNoInteractionAsync(string entityId)
    {
        (await GetRequiredService<ICommentPublicAppService>().GetListAsync(BlogPostConsts.EntityType, entityId)).Items.ShouldBeEmpty();
        (await GetRequiredService<IRatingPublicAppService>().GetGroupedStarCountsAsync(BlogPostConsts.EntityType, entityId)).ShouldBeEmpty();
        (await GetRequiredService<IReactionPublicAppService>().GetForSelectionAsync(BlogPostConsts.EntityType, entityId))
            .Items.ShouldAllBe(item => item.Count == 0);
        (await GetRequiredService<IMarkedItemPublicAppService>().GetForUserAsync(BlogPostConsts.EntityType, entityId))
            .IsMarkedByCurrentUser.ShouldBeFalse();
    }
}
