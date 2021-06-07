using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.BlobStoring;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Guids;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Users;
using Volo.CmsKit.Blogs;
using Volo.CmsKit.Comments;
using Volo.CmsKit.MediaDescriptors;
using Volo.CmsKit.Menus;
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
        private readonly EntityTagManager _entityTagManager;
        private readonly TagManager _tagManager;
        private readonly ITagRepository _tagRepository;
        private readonly IEntityTagRepository _entityTagRepository;
        private readonly IPageRepository _pageRepository;
        private readonly IBlogRepository _blogRepository;
        private readonly IBlogFeatureRepository _blogFeatureRepository;
        private readonly IBlogPostRepository _blogPostRepository;
        private readonly BlogPostManager _blogPostManager;
        private readonly IOptions<CmsKitReactionOptions> _reactionOptions;
        private readonly IOptions<CmsKitTagOptions> _tagOptions;
        private readonly IMediaDescriptorRepository _mediaDescriptorRepository;
        private readonly IBlobContainer<MediaContainer> _mediaBlobContainer;
        private readonly BlogManager _blogManager;
        private readonly IOptions<CmsKitMediaOptions> _mediaOptions;
        private readonly IOptions<CmsKitCommentOptions> _commentsOptions;
        private readonly IOptions<CmsKitRatingOptions> _ratingOptions;
        private readonly IMenuRepository _menuRepository;

        public CmsKitDataSeedContributor(
            IGuidGenerator guidGenerator,
            ICmsUserRepository cmsUserRepository,
            CmsKitTestData cmsKitTestData,
            ICommentRepository commentRepository,
            ReactionManager reactionManager,
            IRatingRepository ratingRepository,
            ICurrentTenant currentTenant,
            TagManager tagManager,
            ITagRepository tagRepository,
            IEntityTagRepository entityTagRepository,
            IPageRepository pageRepository,
            IBlogRepository blogRepository,
            IBlogPostRepository blogPostRepository,
            BlogPostManager blogPostmanager,
            IBlogFeatureRepository blogFeatureRepository,
            EntityTagManager entityTagManager,
            IOptions<CmsKitReactionOptions> reactionOptions,
            IOptions<CmsKitTagOptions> tagOptions,
            IMediaDescriptorRepository mediaDescriptorRepository,
            IBlobContainer<MediaContainer> mediaBlobContainer,
            BlogManager blogManager,
            IOptions<CmsKitMediaOptions> cmsMediaOptions,
            IOptions<CmsKitCommentOptions> commentsOptions,
            IOptions<CmsKitRatingOptions> ratingOptions,
            IMenuRepository menuRepository)
        {
            _guidGenerator = guidGenerator;
            _cmsUserRepository = cmsUserRepository;
            _cmsKitTestData = cmsKitTestData;
            _commentRepository = commentRepository;
            _reactionManager = reactionManager;
            _ratingRepository = ratingRepository;
            _currentTenant = currentTenant;
            _tagManager = tagManager;
            _tagRepository = tagRepository;
            _entityTagManager = entityTagManager;
            _entityTagRepository = entityTagRepository;
            _pageRepository = pageRepository;
            _blogRepository = blogRepository;
            _blogPostRepository = blogPostRepository;
            _blogPostManager = blogPostmanager;
            _blogFeatureRepository = blogFeatureRepository;
            _reactionOptions = reactionOptions;
            _tagOptions = tagOptions;
            _mediaDescriptorRepository = mediaDescriptorRepository;
            _mediaBlobContainer = mediaBlobContainer;
            _blogManager = blogManager;
            _mediaOptions = cmsMediaOptions;
            _commentsOptions = commentsOptions;
            _ratingOptions = ratingOptions;
            _menuRepository = menuRepository;
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

                await SeedTagsAsync();

                await SeedPagesAsync();

                await SeedBlogsAsync();

                await SeedBlogFeaturesAsync();

                await SeedMediaAsync();

                await SeedMenusAsync();
            }
        }

        private Task ConfigureCmsKitOptionsAsync()
        {
            _tagOptions.Value.EntityTypes.AddIfNotContains(new TagEntityTypeDefiniton(_cmsKitTestData.EntityType1));
            _tagOptions.Value.EntityTypes.AddIfNotContains(new TagEntityTypeDefiniton(_cmsKitTestData.EntityType2));
            _tagOptions.Value.EntityTypes.AddIfNotContains(new TagEntityTypeDefiniton(_cmsKitTestData.Content_1_EntityType));
            _tagOptions.Value.EntityTypes.AddIfNotContains(new TagEntityTypeDefiniton(_cmsKitTestData.Content_2_EntityType));
            _tagOptions.Value.EntityTypes.AddIfNotContains(new TagEntityTypeDefiniton(_cmsKitTestData.TagDefinition_1_EntityType));

            _mediaOptions.Value.EntityTypes.AddIfNotContains(
                new MediaDescriptorDefinition(
                    _cmsKitTestData.Media_1_EntityType,
                    createPolicies: new[] { "SomeCreatePolicy" },
                    deletePolicies: new[] { "SomeDeletePolicy" }));

            _commentsOptions.Value.EntityTypes.Add(
                new CommentEntityTypeDefinition(_cmsKitTestData.EntityType1));

            List<ReactionDefinition> reactions = new()
            {
                new ReactionDefinition(StandardReactions.Smile),
                new ReactionDefinition(StandardReactions.ThumbsUp),
                new ReactionDefinition(StandardReactions.ThumbsDown),
                new ReactionDefinition(StandardReactions.Confused),
                new ReactionDefinition(StandardReactions.Eyes),
                new ReactionDefinition(StandardReactions.Heart),
                new ReactionDefinition(StandardReactions.HeartBroken),
                new ReactionDefinition(StandardReactions.Wink),
                new ReactionDefinition(StandardReactions.Pray),
                new ReactionDefinition(StandardReactions.Rocket),
                new ReactionDefinition(StandardReactions.Victory),
                new ReactionDefinition(StandardReactions.Rock),
            };

            _reactionOptions.Value.EntityTypes.Add(new ReactionEntityTypeDefinition(_cmsKitTestData.EntityType1, reactions));
            _reactionOptions.Value.EntityTypes.Add(new ReactionEntityTypeDefinition(_cmsKitTestData.EntityType2, reactions));

            _ratingOptions.Value.EntityTypes.Add(new RatingEntityTypeDefinition(_cmsKitTestData.EntityType1));
            _ratingOptions.Value.EntityTypes.Add(new RatingEntityTypeDefinition(_cmsKitTestData.EntityType2));

            return Task.CompletedTask;
        }

        private async Task SeedUsersAsync()
        {
            await _cmsUserRepository.InsertAsync(new CmsUser(new UserData(_cmsKitTestData.User1Id, "user1",
                "user1@volo.com",
                "user", "1")),
                autoSave: true);

            await _cmsUserRepository.InsertAsync(new CmsUser(new UserData(_cmsKitTestData.User2Id, "user2",
                "user2@volo.com",
                "user", "2")),
                autoSave: true);
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
            await _reactionManager.GetOrCreateAsync(
                _cmsKitTestData.User1Id,
                _cmsKitTestData.EntityType1,
                _cmsKitTestData.EntityId1,
                StandardReactions.Confused);

            await _reactionManager.GetOrCreateAsync(
                _cmsKitTestData.User1Id,
                _cmsKitTestData.EntityType1,
                _cmsKitTestData.EntityId1,
                StandardReactions.ThumbsUp);

            await _reactionManager.GetOrCreateAsync(
                _cmsKitTestData.User1Id,
                _cmsKitTestData.EntityType1,
                _cmsKitTestData.EntityId2,
                StandardReactions.Heart);

            await _reactionManager.GetOrCreateAsync(
                _cmsKitTestData.User1Id,
                _cmsKitTestData.EntityType2,
                _cmsKitTestData.EntityId1,
                StandardReactions.Rocket);

            await _reactionManager.GetOrCreateAsync(
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

        private async Task SeedTagsAsync()
        {
            var created1 = await _tagRepository.InsertAsync(
                await _tagManager.CreateAsync(
                    _cmsKitTestData.TagId_1,
                    _cmsKitTestData.EntityType1,
                    _cmsKitTestData.TagName_1));

            await _entityTagManager.AddTagToEntityAsync(created1.Id, created1.EntityType, _cmsKitTestData.EntityId1);

            var created2 = await _tagRepository.InsertAsync(
                await _tagManager.CreateAsync(
                    _cmsKitTestData.TagId_2,
                    _cmsKitTestData.EntityType2,
                    _cmsKitTestData.TagName_2));

            await _entityTagManager.AddTagToEntityAsync(created2.Id, created2.EntityType, _cmsKitTestData.EntityId2);

            foreach (var tag in _cmsKitTestData.Content_1_Tags)
            {
                var tagEntity = await _tagRepository.InsertAsync(
                    await _tagManager.CreateAsync(
                        _guidGenerator.Create(),
                        _cmsKitTestData.Content_1_EntityType,
                        tag));

                await _entityTagManager.AddTagToEntityAsync(tagEntity.Id, _cmsKitTestData.Content_1_EntityType, _cmsKitTestData.Content_1_EntityId);
            }

            foreach (var tag in _cmsKitTestData.Content_2_Tags)
            {
                var tagEntity = await _tagRepository.InsertAsync(
                    await _tagManager.CreateAsync(
                        _guidGenerator.Create(),
                        _cmsKitTestData.Content_2_EntityType,
                        tag));

                await _entityTagManager.AddTagToEntityAsync(tagEntity.Id, _cmsKitTestData.Content_2_EntityType, _cmsKitTestData.Content_2_EntityId);
            }
        }

        private async Task SeedPagesAsync()
        {
            var page1 = new Page(_cmsKitTestData.Page_1_Id, _cmsKitTestData.Page_1_Title, _cmsKitTestData.Page_1_Slug, _cmsKitTestData.Content_1);
            await _pageRepository.InsertAsync(page1);

            var page2 = new Page(_cmsKitTestData.Page_2_Id, _cmsKitTestData.Page_2_Title, _cmsKitTestData.Page_2_Slug, _cmsKitTestData.Content_2);
            await _pageRepository.InsertAsync(page2);
        }

        private async Task SeedBlogsAsync()
        {
            var blog = await _blogRepository.InsertAsync(
                await _blogManager.CreateAsync(_cmsKitTestData.BlogName, _cmsKitTestData.BlogSlug), autoSave: true);

            _cmsKitTestData.Blog_Id = blog.Id;

            var author = await _cmsUserRepository.GetAsync(_cmsKitTestData.User1Id);

            _cmsKitTestData.BlogPost_1_Id =
                (await _blogPostRepository.InsertAsync(
                    await _blogPostManager.CreateAsync(
                        author,
                        blog,
                        _cmsKitTestData.BlogPost_1_Title,
                        _cmsKitTestData.BlogPost_1_Slug,
                        "Short desc 1",
                        "Blog Post 1 Content"))).Id;

            _cmsKitTestData.BlogPost_2_Id =
                (await _blogPostRepository.InsertAsync(
                    await _blogPostManager.CreateAsync(
                        author,
                        blog,
                        _cmsKitTestData.BlogPost_2_Title,
                        _cmsKitTestData.BlogPost_2_Slug,
                        "Short desc 2",
                        "Blog Post 2 Content"))).Id;
        }

        private async Task SeedBlogFeaturesAsync()
        {
            var blogFeature1 = await _blogFeatureRepository.InsertAsync(
                    new BlogFeature(
                        _cmsKitTestData.Blog_Id,
                        _cmsKitTestData.BlogFeature_1_FeatureName,
                        _cmsKitTestData.BlogFeature_1_Enabled));

            _cmsKitTestData.BlogFeature_1_Id = blogFeature1.Id;

            var blogFeature2 = await _blogFeatureRepository.InsertAsync(
                    new BlogFeature(
                        _cmsKitTestData.Blog_Id,
                        _cmsKitTestData.BlogFeature_2_FeatureName,
                        _cmsKitTestData.BlogFeature_2_Enabled));

            _cmsKitTestData.BlogFeature_2_Id = blogFeature2.Id;
        }

        private async Task SeedMediaAsync()
        {
            using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(_cmsKitTestData.Media_1_Content)))
            {
                var media = new MediaDescriptor(_cmsKitTestData.Media_1_Id, _cmsKitTestData.Media_1_EntityType, _cmsKitTestData.Media_1_Name, _cmsKitTestData.Media_1_ContentType, stream.Length);

                await _mediaDescriptorRepository.InsertAsync(media);

                await _mediaBlobContainer.SaveAsync(media.Id.ToString(), stream);
            }
        }

        private async Task SeedMenusAsync()
        {
            var menu = new Menu(_cmsKitTestData.Menu_1_Id, _cmsKitTestData.Menu_1_Name);

            menu.Items.Add(
                new MenuItem(
                    _cmsKitTestData.MenuItem_1_Id,
                    menu.Id,
                    _cmsKitTestData.MenuItem_1_Name,
                    _cmsKitTestData.MenuItem_1_Url));

            menu.Items.Add(
                new MenuItem(
                    _cmsKitTestData.MenuItem_2_Id,
                    menu.Id,
                    _cmsKitTestData.MenuItem_2_Name,
                    _cmsKitTestData.MenuItem_2_Url));

            await _menuRepository.InsertAsync(menu);
        }
    }
}
