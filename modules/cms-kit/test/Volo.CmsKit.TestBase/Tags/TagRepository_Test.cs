using System;
using System.Linq;
using System.Threading.Tasks;
using Shouldly;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Modularity;
using Xunit;

namespace Volo.CmsKit.Tags;

public abstract class TagRepository_Test<TStartupModule> : CmsKitTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{
    private readonly CmsKitTestData _cmsKitTestData;
    private readonly ITagRepository _tagRepository;

    protected TagRepository_Test()
    {
        _cmsKitTestData = GetRequiredService<CmsKitTestData>();
        _tagRepository = GetRequiredService<ITagRepository>();
    }

    [Theory]
    [InlineData("Lyrics", "Imagine Dragons")]
    [InlineData("Lyrics", "Music")]
    [InlineData("LyricsAlso", "Imagine Dragons")]
    [InlineData("LyricsAlso", "Music")]
    [InlineData("LyricsAlso", "Most Loved Part")]
    public async Task ShouldGetAsync(string entityType, string name)
    {
        var tag = await _tagRepository.GetAsync(entityType, name);

        tag.ShouldNotBeNull();
    }

    [Fact]
    public async Task ShouldNotGetAsync()
    {
        Should.Throw<EntityNotFoundException>(
            async () => await _tagRepository.GetAsync("not_exist_entity", Guid.NewGuid().ToString())
        );
    }

    [Theory]
    [InlineData("Lyrics", "Imagine Dragons")]
    [InlineData("Lyrics", "Music")]
    [InlineData("LyricsAlso", "Imagine Dragons")]
    [InlineData("LyricsAlso", "Music")]
    [InlineData("LyricsAlso", "Most Loved Part")]
    public async Task ShouldExistAsync(string entityType, string name)
    {
        var tag = await _tagRepository.AnyAsync(entityType, name);

        tag.ShouldBeTrue();
    }

    [Fact]
    public async Task ShouldNotExistAsync()
    {
        var tag = await _tagRepository.AnyAsync("not_exist_entity", Guid.NewGuid().ToString());

        tag.ShouldBeFalse();
    }

    [Theory]
    [InlineData("Lyrics", "Imagine Dragons")]
    [InlineData("Lyrics", "Music")]
    [InlineData("LyricsAlso", "Imagine Dragons")]
    [InlineData("LyricsAlso", "Music")]
    [InlineData("LyricsAlso", "Most Loved Part")]
    public async Task ShouldFindAsync(string entityType, string name)
    {
        var tag = await _tagRepository.FindAsync(entityType, name);

        tag.ShouldNotBeNull();
    }

    [Fact]
    public async Task ShouldNotFindAsync()
    {
        var tag = await _tagRepository.FindAsync("not_exist_entity", Guid.NewGuid().ToString());

        tag.ShouldBeNull();
    }

    [Fact]
    public async Task Should_Get_Related_Tags()
    {
        var content_1_tags = await _tagRepository.GetAllRelatedTagsAsync(_cmsKitTestData.Content_1_EntityType, _cmsKitTestData.EntityId1);

        content_1_tags.Count.ShouldBe(_cmsKitTestData.Content_1_Tags.Length);

        foreach (var tag in content_1_tags)
        {
            _cmsKitTestData.Content_1_Tags.Contains(tag.Name).ShouldBeTrue();
        }

        var content_2_tags = await _tagRepository.GetAllRelatedTagsAsync(_cmsKitTestData.Content_2_EntityType, _cmsKitTestData.EntityId2);

        content_2_tags.Count.ShouldBe(_cmsKitTestData.Content_2_Tags.Length);

        foreach (var tag in content_2_tags)
        {
            _cmsKitTestData.Content_2_Tags.Contains(tag.Name).ShouldBeTrue();
        }
    }

    [Fact]
    public async Task Should_Not_Get_Related_Tags()
    {
        var tags = await _tagRepository.GetAllRelatedTagsAsync("not_exist_entity", Guid.NewGuid().ToString());

        tags.Count.ShouldBe(0);
    }

    [Fact]
    public async Task Should_GetList_With_Filter()
    {
        var tags = await _tagRepository.GetListAsync(_cmsKitTestData.TagName_1);

        tags.ShouldNotBeNull();
        tags.Count.ShouldBe(1);
    }

    [Fact]
    public async Task Should_GetCount_With_Filter()
    {
        var count = await _tagRepository.GetCountAsync(_cmsKitTestData.TagName_1);

        count.ShouldBe(1);
    }
}
