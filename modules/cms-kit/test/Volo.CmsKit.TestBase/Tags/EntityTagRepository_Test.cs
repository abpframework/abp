using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Modularity;
using Xunit;

namespace Volo.CmsKit.Tags
{
    public abstract class EntityTagRepository_Test<TStartupModule> : CmsKitTestBase<TStartupModule>
        where TStartupModule : IAbpModule
    {
        private CmsKitTestData _cmsKitTestData;
        private IEntityTagRepository _entityTagRepository;
        private ITagRepository _tagRepository;

        public EntityTagRepository_Test()
        {
            _cmsKitTestData = GetRequiredService<CmsKitTestData>();
            _entityTagRepository = GetRequiredService<IEntityTagRepository>();
            _tagRepository = GetRequiredService<ITagRepository>();
        }

        [Fact]
        public async Task DeleteManyAsync_ShouldWorkProperly_WithExistingIds()
        {
            var relatedTags = await _tagRepository.GetAllRelatedTagsAsync(_cmsKitTestData.Content_1_EntityType, _cmsKitTestData.Content_1_EntityId);

            await _entityTagRepository.DeleteManyAsync(relatedTags.Select(s => s.Id).ToArray());

            relatedTags = await _tagRepository.GetAllRelatedTagsAsync(_cmsKitTestData.Content_1_EntityType, _cmsKitTestData.Content_1_EntityId);

            relatedTags.ShouldBeEmpty();
        }
    }
}
