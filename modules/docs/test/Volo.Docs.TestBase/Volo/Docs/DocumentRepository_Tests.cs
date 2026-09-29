using System;
using System.Linq;
using System.Threading.Tasks;
using Shouldly;
using Volo.Abp.Modularity;
using Volo.Docs.Documents;
using Xunit;

namespace Volo.Docs
{
    public abstract class DocumentRepository_Tests<TStartupModule> : DocsTestBase<TStartupModule>
        where TStartupModule : IAbpModule
    {
        protected readonly IDocumentRepository DocumentRepository;
        protected readonly DocsTestData DocsTestData;

        protected DocumentRepository_Tests()
        {
            DocumentRepository = GetRequiredService<IDocumentRepository>();
            DocsTestData = GetRequiredService<DocsTestData>();
        }

        [Fact]
        public async Task FindAsync()
        {
            var document = await DocumentRepository.FindAsync(DocsTestData.ProjectId, "CLI.md", "en", "2.0.0");
            document.ShouldNotBeNull();
        }

        [Fact]
        public async Task DeleteAsync()
        {
            (await DocumentRepository.GetListAsync()).ShouldNotBeEmpty();

            await DocumentRepository.DeleteAsync(DocsTestData.ProjectId, "CLI.md", "en", "2.0.0");

            (await DocumentRepository.GetListAsync()).ShouldBeEmpty();
        }
        
        [Fact]
        public async Task UpdateProjectLastCachedTimeAsync()
        {
            await DocumentRepository.UpdateProjectLastCachedTimeAsync(DocsTestData.ProjectId, DateTime.MinValue);
            var documentsAfterClear = await DocumentRepository.GetListByProjectId(DocsTestData.ProjectId);
            documentsAfterClear.ForEach(d => d.LastCachedTime.ShouldBe(DateTime.MinValue));
        }
        
        [Fact] 
        public async Task GetUniqueDocumentsByProjectIdPagedAsync()
        {
            var documents = await DocumentRepository.GetUniqueDocumentsByProjectIdPagedAsync(DocsTestData.ProjectId, 0, 10);
            documents.Count.ShouldBe(1);
        }
        
        [Fact]
        public async Task GetUniqueDocumentCountByProjectIdAsync()
        {
            var count = await DocumentRepository.GetUniqueDocumentCountByProjectIdAsync(DocsTestData.ProjectId);
            count.ShouldBe(1);
        }

        [Fact]
        public async Task GetUniqueDocumentsByProjectIdPagedAsync_Should_Page_Groups_In_Order()
        {
            var now = DateTime.Now;
            await InsertDocumentAsync("guide/A.md", "a-newest", now.AddDays(-1));
            await InsertDocumentAsync("guide/A.md", "a-oldest", now.AddDays(-9));
            await InsertDocumentAsync("guide/A.md", "a-middle", now.AddDays(-5));
            await InsertDocumentAsync("B.md", "b", now);
            await InsertDocumentAsync("other/B.md", "other-b", now);

            (await DocumentRepository.GetUniqueDocumentCountByProjectIdAsync(DocsTestData.ProjectId)).ShouldBe(4);

            var firstPage = await DocumentRepository.GetUniqueDocumentsByProjectIdPagedAsync(DocsTestData.ProjectId, 0, 2);
            firstPage.Select(x => x.Name).ShouldBe(new[] { "B.md", "CLI.md" });

            var secondPage = await DocumentRepository.GetUniqueDocumentsByProjectIdPagedAsync(DocsTestData.ProjectId, 2, 2);
            secondPage.Select(x => x.Content).ShouldBe(new[] { "a-oldest", "other-b" });
        }

        private async Task InsertDocumentAsync(string name, string content, DateTime lastCachedTime)
        {
            await DocumentRepository.InsertAsync(new Document(Guid.NewGuid(), DocsTestData.ProjectId, name, "2.0.0", "en",
                name.Substring(name.LastIndexOf('/') + 1), content, "md", null, "https://github.com/abpframework/abp/tree/2.0.0/docs/",
                "https://raw.githubusercontent.com/abpframework/abp/2.0.0/docs/en/", "", lastCachedTime, lastCachedTime,
                lastCachedTime), autoSave: true);
        }
    }
}
