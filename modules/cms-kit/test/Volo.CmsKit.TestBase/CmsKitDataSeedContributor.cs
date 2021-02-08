using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Guids;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Users;
using Volo.CmsKit.Comments;
using Volo.CmsKit.Contents;
using Volo.CmsKit.Pages;
using Volo.CmsKit.Ratings;
using Volo.CmsKit.Reactions;
using Volo.CmsKit.Tags;
using Volo.CmsKit.Users;

namespace Volo.CmsKit
{
    public class CmsKitDataSeedContributor : IDataSeedContributor, ITransientDependency
    {
        private readonly IGuidGenerator _guidGenerator;
        private readonly ICmsUserRepository _cmsUserRepository;
        private readonly CmsKitTestData _cmsKitTestData;
        private readonly ICommentRepository _commentRepository;
        private readonly ReactionManager _reactionManager;
        private readonly IRatingRepository _ratingRepository;
        private readonly ICurrentTenant _currentTenant;
        private readonly IContentRepository _contentRepository;
        private readonly IEntityTagManager _entityTagManager;
        private readonly ITagManager _tagManager;
        private readonly IPageRepository _pageRepository;
        private readonly IOptions<CmsKitOptions> _options;
        private readonly IOptions<CmsKitTagOptions> _tagOptions;

        public CmsKitDataSeedContributor(
            IGuidGenerator guidGenerator,
            ICmsUserRepository cmsUserRepository,
            CmsKitTestData cmsKitTestData,
            ICommentRepository commentRepository,
            ReactionManager reactionManager,
            IRatingRepository ratingRepository,
            ICurrentTenant currentTenant,
            IContentRepository contentRepository,
            ITagManager tagManager,
            IEntityTagManager entityTagManager,
            IPageRepository pageRepository,
            IOptions<CmsKitOptions> options,
            IOptions<CmsKitTagOptions> tagOptions)
        {
            _guidGenerator = guidGenerator;
            _cmsUserRepository = cmsUserRepository;
            _cmsKitTestData = cmsKitTestData;
            _commentRepository = commentRepository;
            _reactionManager = reactionManager;
            _ratingRepository = ratingRepository;
            _currentTenant = currentTenant;
            _contentRepository = contentRepository;
            _tagManager = tagManager;
            _entityTagManager = entityTagManager;
            _pageRepository = pageRepository;
            _options = options;
            _tagOptions = tagOptions;
        }

        public async Task SeedAsync(DataSeedContext context)
        {
            using (_currentTenant.Change(context?.TenantId))
            {
                await ConfigureCmsKitOptionsAsync();

                await SeedUsersAsync();

                await SeedCommentsAsync();

                await SeedReactionsAsync();

                await SeedRatingsAsync();

                await SeedContentsAsync();

                await SeedTagsAsync();

                await SeedPagesAsync();
            }
        }

        private Task ConfigureCmsKitOptionsAsync()
        {
            _tagOptions.Value.EntityTypes.AddIfNotContains(new TagEntityTypeDefiniton(_cmsKitTestData.EntityType1));
            _tagOptions.Value.EntityTypes.AddIfNotContains(new TagEntityTypeDefiniton(_cmsKitTestData.EntityType2));
            _tagOptions.Value.EntityTypes.AddIfNotContains(new TagEntityTypeDefiniton(_cmsKitTestData.Content_1_EntityType));
            _tagOptions.Value.EntityTypes.AddIfNotContains(new TagEntityTypeDefiniton(_cmsKitTestData.Content_2_EntityType));
            _tagOptions.Value.EntityTypes.AddIfNotContains(new TagEntityTypeDefiniton(_cmsKitTestData.TagDefinition_1_EntityType));

            return Task.CompletedTask;
        }

        private async Task SeedUsersAsync()
        {
            await _cmsUserRepository.InsertAsync(new CmsUser(new UserData(_cmsKitTestData.User1Id, "user1",
                "user1@volo.com",
                "user", "1")));
            await _cmsUserRepository.InsertAsync(new CmsUser(new UserData(_cmsKitTestData.User2Id, "user2",
                "user2@volo.com",
                "user", "2")));
        }

        private async Task SeedCommentsAsync()
        {
            var comment1 = await _commentRepository.InsertAsync(new Comment(_cmsKitTestData.CommentWithChildId,
                _cmsKitTestData.EntityType1,
                _cmsKitTestData.EntityId1,
                "comment",
                null,
                _cmsKitTestData.User1Id
            ));

            await _commentRepository.InsertAsync(new Comment(_guidGenerator.Create(),
                _cmsKitTestData.EntityType1,
                _cmsKitTestData.EntityId1,
                "reply",
                comment1.Id,
                _cmsKitTestData.User2Id
            ));

            await _commentRepository.InsertAsync(new Comment(_guidGenerator.Create(),
                _cmsKitTestData.EntityType1,
                _cmsKitTestData.EntityId1,
                "reply",
                comment1.Id,
                _cmsKitTestData.User1Id
            ));

            await _commentRepository.InsertAsync(new Comment(_guidGenerator.Create(),
                _cmsKitTestData.EntityType1,
                _cmsKitTestData.EntityId1,
                "comment",
                null,
                _cmsKitTestData.User2Id
            ));

            await _commentRepository.InsertAsync(new Comment(_guidGenerator.Create(),
                _cmsKitTestData.EntityType1,
                _cmsKitTestData.EntityId2,
                "comment",
                null,
                _cmsKitTestData.User2Id
            ));

            await _commentRepository.InsertAsync(new Comment(_guidGenerator.Create(),
                _cmsKitTestData.EntityType2,
                _cmsKitTestData.EntityId1,
                "comment",
                null,
                _cmsKitTestData.User2Id
            ));
        }

        private async Task SeedReactionsAsync()
        {
            await _reactionManager.CreateAsync(
                _cmsKitTestData.User1Id,
                _cmsKitTestData.EntityType1,
                _cmsKitTestData.EntityId1,
                StandardReactions.Confused);

            await _reactionManager.CreateAsync(
                _cmsKitTestData.User1Id,
                _cmsKitTestData.EntityType1,
                _cmsKitTestData.EntityId1,
                StandardReactions.ThumbsUp);

            await _reactionManager.CreateAsync(
                _cmsKitTestData.User1Id,
                _cmsKitTestData.EntityType1,
                _cmsKitTestData.EntityId2,
                StandardReactions.Heart);

            await _reactionManager.CreateAsync(
                _cmsKitTestData.User1Id,
                _cmsKitTestData.EntityType2,
                _cmsKitTestData.EntityId1,
                StandardReactions.Rocket);

            await _reactionManager.CreateAsync(
                _cmsKitTestData.User2Id,
                _cmsKitTestData.EntityType1,
                _cmsKitTestData.EntityId1,
                StandardReactions.ThumbsUp);
        }

        private async Task SeedRatingsAsync()
        {
            await _ratingRepository.InsertAsync(new Rating(_guidGenerator.Create(),
                    _cmsKitTestData.EntityType1,
                    _cmsKitTestData.EntityId1,
                    4,
                    _cmsKitTestData.User1Id
                ));

            await _ratingRepository.InsertAsync(new Rating(_guidGenerator.Create(),
                _cmsKitTestData.EntityType1,
                _cmsKitTestData.EntityId1,
                5,
                _cmsKitTestData.User1Id
            ));

            await _ratingRepository.InsertAsync(new Rating(_guidGenerator.Create(),
                _cmsKitTestData.EntityType2,
                _cmsKitTestData.EntityId2,
                5,
                _cmsKitTestData.User2Id
            ));

            await _ratingRepository.InsertAsync(new Rating(_guidGenerator.Create(),
                _cmsKitTestData.EntityType2,
                _cmsKitTestData.EntityId2,
                1,
                _cmsKitTestData.User2Id
            ));
        }

        private async Task SeedContentsAsync()
        {
            var content1 = new Content(
                _cmsKitTestData.Content_1_Id,
                _cmsKitTestData.Content_1_EntityType,
                _cmsKitTestData.Content_1_EntityId,
                _cmsKitTestData.Content_1
                );

            var content2 = new Content(
                _cmsKitTestData.Content_2_Id,
                _cmsKitTestData.Content_2_EntityType,
                _cmsKitTestData.Content_2_EntityId,
                _cmsKitTestData.Content_2
            );

            var content3 = new Content(
                Guid.NewGuid(),
                "deleted_entity_type",
                "deleted_entity_id",
                "Content"
            )
            {
                IsDeleted = true,
            };

            await _contentRepository.InsertAsync(content1);
            await _contentRepository.InsertAsync(content2);
            await _contentRepository.InsertAsync(content3);
        }

        private async Task SeedTagsAsync()
        {
            var created1 = await _tagManager.InsertAsync(_cmsKitTestData.TagId_1, _cmsKitTestData.EntityType1, _cmsKitTestData.TagName_1);

            await _entityTagManager.AddTagToEntityAsync(created1.Id, created1.EntityType, _cmsKitTestData.EntityId1);

            var created2 = await _tagManager.InsertAsync(_cmsKitTestData.TagId_2, _cmsKitTestData.EntityType2, _cmsKitTestData.TagName_2);

            await _entityTagManager.AddTagToEntityAsync(created2.Id, created2.EntityType, _cmsKitTestData.EntityId2);

            foreach (var tag in _cmsKitTestData.Content_1_Tags)
            {
                var tagEntity = await _tagManager.InsertAsync(_guidGenerator.Create(), _cmsKitTestData.Content_1_EntityType, tag);

                await _entityTagManager.AddTagToEntityAsync(tagEntity.Id, _cmsKitTestData.Content_1_EntityType, _cmsKitTestData.Content_1_EntityId);
            }

            foreach (var tag in _cmsKitTestData.Content_2_Tags)
            {
                var tagEntity = await _tagManager.InsertAsync(_guidGenerator.Create(), _cmsKitTestData.Content_2_EntityType, tag);

                await _entityTagManager.AddTagToEntityAsync(tagEntity.Id, _cmsKitTestData.Content_2_EntityType, _cmsKitTestData.Content_2_EntityId);
            }
        }

        private async Task SeedPagesAsync()
        {
            var page1 = new Page(_cmsKitTestData.Page_1_Id, _cmsKitTestData.Page_1_Title, _cmsKitTestData.Page_1_Url, _cmsKitTestData.Page_1_Description);
            var page1Content = new Content(_guidGenerator.Create(), nameof(Page), page1.Id.ToString(), _cmsKitTestData.Page_1_Content);

            await _pageRepository.InsertAsync(page1);
            await _contentRepository.InsertAsync(page1Content);

            var page2 = new Page(_cmsKitTestData.Page_2_Id, _cmsKitTestData.Page_2_Title, _cmsKitTestData.Page_2_Url, _cmsKitTestData.Page_2_Description);
            var page2Content = new Content(_guidGenerator.Create(), nameof(Page), page2.Id.ToString(), _cmsKitTestData.Page_2_Content);

            await _pageRepository.InsertAsync(page2);
            await _contentRepository.InsertAsync(page2Content);
        }
    }
}
