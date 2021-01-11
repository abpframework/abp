using Shouldly;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Guids;
using Xunit;

namespace Volo.CmsKit.Tags
{
    public class EntityTagManager_Tests : CmsKitDomainTestBase
    {
        private readonly CmsKitTestData _cmsKitTestData;
        private readonly IEntityTagManager _entityTagManager;
        private readonly ITagRepository _tagRepository;
        private readonly IGuidGenerator _guidGenerator;

        public EntityTagManager_Tests()
        {
            _cmsKitTestData = GetRequiredService<CmsKitTestData>();
            _entityTagManager = GetRequiredService<IEntityTagManager>();
            _tagRepository = GetRequiredService<ITagRepository>();
            _guidGenerator = GetRequiredService<IGuidGenerator>();
        }

        [Fact]
        public async Task AddTagToEntityAsync_ShouldAdd_WhenEverythingCorrect()
        {
            var tag = await _tagRepository.InsertAsync(new Tag(_guidGenerator.Create(), _cmsKitTestData.EntityType1, "My Test Tag #1"));

            var entityTag = await _entityTagManager.AddTagToEntityAsync(tag.Id, _cmsKitTestData.EntityType1, _cmsKitTestData.EntityId1);

            entityTag.ShouldNotBeNull();
        }

        [Fact]
        public async Task AddTagToEntity_ShouldThrowNotTaggable_WithNotConfiguredEntityType()
        {
            var entityType = "Not.Configured.EntityType";

            var exception = Should.Throw<EntityNotTaggableException>(async () =>
                await _entityTagManager.AddTagToEntityAsync(_cmsKitTestData.TagId_1, entityType, _cmsKitTestData.EntityId1)
            );

            exception.ShouldNotBeNull();
            exception.Data[nameof(Tag.EntityType)].ShouldBe(entityType);
        }

        [Fact]
        public async Task RemoveTagFromEntityAsync_ShouldRemove_WhenEverythingCorrect()
        {
            var tagToDelete = (await _tagRepository.GetAllRelatedTagsAsync(_cmsKitTestData.EntityType1, _cmsKitTestData.EntityId1))
                                .First();

            await _entityTagManager.RemoveTagFromEntityAsync(tagToDelete.Id, tagToDelete.EntityType, _cmsKitTestData.EntityId1);

            var tags = await _tagRepository.GetAllRelatedTagsAsync(_cmsKitTestData.EntityType1, _cmsKitTestData.EntityId1);

            tags.ShouldNotContain(x => x.Id == tagToDelete.Id);
        }
    }
}
