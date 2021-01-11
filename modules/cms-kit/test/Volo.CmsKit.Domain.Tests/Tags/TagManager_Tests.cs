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
        public async Task ShouldInsertAsync()
        {
            var tagName = "Freshly Created New Tag";
            var tag = await _tagManager.InsertAsync(Guid.NewGuid(), _cmsKitTestData.EntityType1, tagName);

            tag.ShouldNotBeNull();

            var doesExist = await _tagRepository.AnyAsync(_cmsKitTestData.EntityType1, tagName);
            
            doesExist.ShouldBeTrue();
        }
        [Fact]
        public async Task ShouldntInsertWithUnconfiguredEntityTypeAsync()
        {
            var notConfiguredEntityType = "My.Namespace.SomeEntity";

            var exception = await Should.ThrowAsync<EntityNotTaggableException>(async () => 
                await _tagManager.InsertAsync(Guid.NewGuid(), notConfiguredEntityType, "test"));

            exception.ShouldNotBeNull();
            exception.Data[nameof(Tag.EntityType)].ShouldBe(notConfiguredEntityType);
        }
        
        [Fact]
        public async Task ShouldNotInsert()
        {
            var type = _cmsKitTestData.Content_1_EntityType;
            var name = _cmsKitTestData.Content_1_Tags[0];

            Should.Throw<Exception>(async () => await _tagManager.InsertAsync(Guid.NewGuid(), type, name));
        }
        
        [Fact]
        public async Task ShouldUpdate()
        {
            var type = _cmsKitTestData.Content_1_EntityType;
            var name = _cmsKitTestData.Content_1_Tags[0];
            var newName = name + "-updated";
            
            var tag = await _tagRepository.GetAsync(type, name);

            await _tagManager.UpdateAsync(tag.Id, newName);

            var updatedTag = await _tagRepository.GetAsync(type, newName);
            
            updatedTag.Id.ShouldBe(tag.Id);
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

        [Fact]
        public async Task ShouldGetTagDefinitionsProperly_WithoutParameter()
        {
            var definitions = await _tagManager.GetTagDefinitionsAsync();

            definitions.ShouldNotBeNull();
            definitions.Count.ShouldBeGreaterThan(1);
            definitions.ShouldContain(x => x.EntityType == _cmsKitTestData.TagDefinition_1_EntityType);
        }
    }
}