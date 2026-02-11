using System;
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
    }
}
