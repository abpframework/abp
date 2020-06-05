using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shouldly;
using Volo.Abp.Modularity;
using Xunit;

namespace Volo.Blogging.Tagging
{
    public abstract class TagRepository_Tests<TStartupModule> : BloggingTestBase<TStartupModule>
        where TStartupModule : IAbpModule
    {
        protected ITagRepository TagRepository { get; }
        protected BloggingTestData BloggingTestData { get; }

        protected TagRepository_Tests()
        {
            TagRepository = GetRequiredService<ITagRepository>();
            BloggingTestData = GetRequiredService<BloggingTestData>();
        }

        [Fact]
        public async Task GetListAsync()
        {
            var tags =  await TagRepository.GetListAsync(BloggingTestData.Blog1Id);
            tags.ShouldNotBeNull();
            tags.Count.ShouldBe(2);
        }

        [Fact]
        public async Task GetByNameAsync()
        {
            var tag = await TagRepository.GetByNameAsync(BloggingTestData.Blog1Id, BloggingTestData.Tag1Name);
            tag.ShouldNotBeNull();
            tag.Name.ShouldBe(BloggingTestData.Tag1Name);
        }

        [Fact]
        public async Task FindByNameAsync()
        {
            var tag = await TagRepository.FindByNameAsync(BloggingTestData.Blog1Id, BloggingTestData.Tag1Name);
            tag.ShouldNotBeNull();
            tag.Name.ShouldBe(BloggingTestData.Tag1Name);
        }

        [Fact]
        public async Task GetListAsync2()
        {
            var tagIds = (await TagRepository.GetListAsync()).Select(x => x.Id).ToList();
            var tags = await TagRepository.GetListAsync(tagIds);
            tags.ShouldNotBeNull();
            tags.Count.ShouldBe(tagIds.Count);
        }

        [Fact]
        public async Task DecreaseUsageCountOfTags()
        {
            var tag = await TagRepository.FindByNameAsync(BloggingTestData.Blog1Id, BloggingTestData.Tag1Name);
            var usageCount = tag.UsageCount;

            await TagRepository.DecreaseUsageCountOfTagsAsync(new List<Guid>()
            {
                tag.Id
            });

            await TagRepository.FindByNameAsync(BloggingTestData.Blog1Id, BloggingTestData.Tag1Name);
            (await TagRepository.FindByNameAsync(BloggingTestData.Blog1Id, BloggingTestData.Tag1Name)).UsageCount
                .ShouldBe(usageCount - 1);
        }
    }
}
