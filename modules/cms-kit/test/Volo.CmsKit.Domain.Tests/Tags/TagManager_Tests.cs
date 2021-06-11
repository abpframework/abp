using System;
using System.Threading.Tasks;
using Shouldly;
using Xunit;

namespace Volo.CmsKit.Tags
{
    public class TagManager_Tests : CmsKitDomainTestBase
    {
        private readonly CmsKitTestData _cmsKitTestData;
        private readonly TagManager _tagManager;

        private readonly ITagRepository _tagRepository;
        
        public TagManager_Tests()
        {
            _cmsKitTestData = GetRequiredService<CmsKitTestData>();
            _tagManager = GetRequiredService<TagManager>();
            _tagRepository = GetRequiredService<ITagRepository>();
        }

        [Fact]
        public async Task ShouldAddWhenGettingAsync()
        {
            var newTagEntityType = _cmsKitTestData.EntityType1;
            var newTagName = "test_tag_2123";
            
            var doesExist = await _tagRepository.AnyAsync(newTagEntityType, newTagName);
            
            doesExist.ShouldBeFalse();
            
            var newTag = await _tagManager.GetOrAddAsync(newTagEntityType, newTagName);

            newTag.ShouldNotBeNull();
            
            var doesExistAgain = await _tagRepository.AnyAsync(newTagEntityType, newTagName);
            
            doesExistAgain.ShouldBeTrue();
        }
        
        [Fact]
        public async Task ShouldNotAddWhenGettingAsync()
        {
            var tagName = _cmsKitTestData.Content_1_Tags[0];

            var tag = await _tagRepository.GetAsync(_cmsKitTestData.Content_1_EntityType, tagName);
            
            var newTag = await _tagManager.GetOrAddAsync(_cmsKitTestData.Content_1_EntityType, tag.Name);

            newTag.ShouldNotBeNull();
            
            newTag.Id.ShouldBe(tag.Id);
        }

        [Fact]
        public async Task ShouldCreateAsync()
        {
            var tagName = "Freshly Created New Tag";
            var tag = await _tagManager.CreateAsync(Guid.NewGuid(), _cmsKitTestData.EntityType1, tagName);

            tag.ShouldNotBeNull();

            tag.Id.ShouldNotBe(Guid.Empty);
        }

        [Fact]
        public async Task ShouldntInsertWithUnconfiguredEntityTypeAsync()
        {
            var notConfiguredEntityType = "My.Namespace.SomeEntity";

            var exception = await Should.ThrowAsync<EntityNotTaggableException>(async () => 
                await _tagManager.CreateAsync(Guid.NewGuid(), notConfiguredEntityType, "test"));

            exception.ShouldNotBeNull();
            exception.Data[nameof(Tag.EntityType)].ShouldBe(notConfiguredEntityType);
        }
        
        [Fact]
        public async Task ShouldNotInsert()
        {
            var type = _cmsKitTestData.Content_1_EntityType;
            var name = _cmsKitTestData.Content_1_Tags[0];

            Should.Throw<Exception>(async () => await _tagManager.CreateAsync(Guid.NewGuid(), type, name));
        }
        
        [Fact]
        public async Task ShouldUpdate()
        {
            var type = _cmsKitTestData.Content_1_EntityType;
            var name = _cmsKitTestData.Content_1_Tags[0];
            var newName = name + "-updated";
            
            var tag = await _tagRepository.GetAsync(type, name);

            var updatedTag = await _tagManager.UpdateAsync(tag.Id, newName);
            
            updatedTag.Id.ShouldBe(tag.Id);
            updatedTag.Name.ShouldBe(newName);
        }
        
        [Fact]
        public async Task ShouldNotUpdate()
        {
            var type = _cmsKitTestData.Content_1_EntityType;
            var name = _cmsKitTestData.Content_1_Tags[0];
            var newName = _cmsKitTestData.Content_1_Tags[1];
            
            var tag = await _tagRepository.GetAsync(type, name);

            Should.Throw<Exception>(async () => await _tagManager.UpdateAsync(tag.Id, newName));
        }
    }
}