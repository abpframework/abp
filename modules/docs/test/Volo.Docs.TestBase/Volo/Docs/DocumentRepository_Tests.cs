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
            var document = await DocumentRepository.FindAsync(DocsTestData.PorjectId, "CLI.md", "en", "2.0.0");
            document.ShouldNotBeNull();
        }

        [Fact]
        public async Task DeleteAsync()
        {
            (await DocumentRepository.GetListAsync()).ShouldNotBeEmpty();

            await DocumentRepository.DeleteAsync(DocsTestData.PorjectId, "CLI.md", "en", "2.0.0");

            (await DocumentRepository.GetListAsync()).ShouldBeEmpty();
        }
    }
}
