using System;
using System.Threading.Tasks;
using Shouldly;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Modularity;
using Xunit;

namespace Volo.CmsKit.Contents
{
    public abstract class ContentRepository_Tests<TStartupModule> : CmsKitTestBase<TStartupModule>
        where TStartupModule : IAbpModule
    {
        private readonly CmsKitTestData _cmsKitTestData;
        private readonly IContentRepository _contentRepository;

        protected ContentRepository_Tests()
        {
            _cmsKitTestData = GetRequiredService<CmsKitTestData>();
            _contentRepository = GetRequiredService<IContentRepository>();
        }

        [Theory]
        [InlineData("Lyrics", "1", "First things first")]
        [InlineData("LyricsAlso", "2", "Second thing second")]
        public async Task ShouldGetAsync(string entityType, string entityId, string contentStartsWith = null)
        {
            var content = await _contentRepository.GetAsync(entityType, entityId);

            content.ShouldNotBeNull();

            if (contentStartsWith != null)
            {
                content.Value.ShouldStartWith(contentStartsWith);
            }
        }

        [Fact]
        public async Task ShouldNotGetAsync()
        {
            Should.Throw<EntityNotFoundException>(
                async ()
                    =>
                    await _contentRepository.GetAsync("not_exist_entity", Guid.NewGuid().ToString()));
        }

        [Fact]
        public async Task ShouldFindAsync()
        {
            var content =
                await _contentRepository.FindAsync(_cmsKitTestData.Content_1_EntityType, _cmsKitTestData.Content_1_EntityId);

            content.ShouldNotBeNull();
        }

        [Fact]
        public async Task ShouldNotFindAsync()
        {
            var content = await _contentRepository.FindAsync("not_exist_entity", Guid.NewGuid().ToString());

            content.ShouldBeNull();
        }
    }
}