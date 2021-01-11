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
            var newTagEntityType = "testEntity";
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
        public async Task ShouldInsert()
        {
            var tag = await _tagManager.InsertAsync(Guid.NewGuid(), "test", "test");

            tag.ShouldNotBeNull();

            var doesExist = await _tagRepository.AnyAsync("test", "test");
            
            doesExist.ShouldBeTrue();
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
    }
}