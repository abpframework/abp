using System.Linq;
using System.Threading.Tasks;
using Shouldly;
using Volo.Abp.Modularity;
using Xunit;

namespace Volo.CmsKit.Tags;

public abstract class EntityTagRepository_Test<TStartupModule> : CmsKitTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{
    private readonly CmsKitTestData _cmsKitTestData;
    private readonly IEntityTagRepository _entityTagRepository;
    private readonly ITagRepository _tagRepository;

    protected EntityTagRepository_Test()
    {
        _cmsKitTestData = GetRequiredService<CmsKitTestData>();
        _entityTagRepository = GetRequiredService<IEntityTagRepository>();
        _tagRepository = GetRequiredService<ITagRepository>();
    }

    [Fact]
    public async Task DeleteManyAsync_ShouldWorkProperly_WithExistingIds()
    {
        var relatedTags = await _tagRepository.GetAllRelatedTagsAsync(_cmsKitTestData.Content_1_EntityType, _cmsKitTestData.Content_1_EntityId);

        await _entityTagRepository.DeleteManyAsync(relatedTags.Select(s => s.Id).ToArray(), _cmsKitTestData.Content_1_EntityId);

        relatedTags = await _tagRepository.GetAllRelatedTagsAsync(_cmsKitTestData.Content_1_EntityType, _cmsKitTestData.Content_1_EntityId);

        relatedTags.ShouldBeEmpty();
    }

    [Fact]
    public async Task GetEntityIdsFilteredByTagNameAsync_ShouldWorkProperly()
    {
        var entityIds = await _entityTagRepository.GetEntityIdsFilteredByTagNameAsync(_cmsKitTestData.TagName_1, _cmsKitTestData.EntityType1);

        entityIds.ShouldNotBeNull();
        entityIds.ShouldNotBeEmpty();

        entityIds.Count.ShouldBe(1);
    }
}
