using System.Linq;
using System.Threading.Tasks;
using Shouldly;
using Volo.Docs.Admin.Documents;
using Volo.Docs.Documents;
using Xunit;

namespace Volo.Docs
{
    public class DocumentAdminAppService_Tests : DocsAdminApplicationTestBase
    {
        private readonly IDocumentAdminAppService _documentAdminAppService;
        private readonly IDocumentRepository _documentRepository;
        private readonly DocsTestData _testData;

        public DocumentAdminAppService_Tests()
        {
            _documentAdminAppService = GetRequiredService<IDocumentAdminAppService>();
            _documentRepository = GetRequiredService<IDocumentRepository>();
            _testData = GetRequiredService<DocsTestData>();
        }

        [Fact]
        public async Task PullAsync()
        {
            (await _documentRepository.FindAsync(_testData.ProjectId, "Part-I.md", "en", "1.0.0")).ShouldBeNull();

            await _documentAdminAppService.PullAsync(new PullDocumentInput
            {
                ProjectId = _testData.ProjectId,
                Name = "Part-I.md",
                LanguageCode = "en",
                Version = "1.0.0"
            });

            (await _documentRepository.FindAsync(_testData.ProjectId, "Part-I.md", "en", "1.0.0")).ShouldNotBeNull();
        }

        [Fact]
        public async Task PullAllAsync()
        {
            (await _documentRepository.FindAsync(_testData.ProjectId, "Part-I.md", "en", "1.0.0")).ShouldBeNull();
            (await _documentRepository.FindAsync(_testData.ProjectId, "Part-II.md", "en", "1.0.0")).ShouldBeNull();

            await _documentAdminAppService.PullAllAsync(new PullAllDocumentInput
            {
                ProjectId = _testData.ProjectId,
                LanguageCode = "en",
                Version = "1.0.0"
            });

            (await _documentRepository.FindAsync(_testData.ProjectId, "Part-I.md", "en", "1.0.0")).ShouldNotBeNull();
            (await _documentRepository.FindAsync(_testData.ProjectId, "Part-II.md", "en", "1.0.0")).ShouldNotBeNull();
        }

        [Fact]
        public async Task GetFilterItemsAsync()
        {
            var filterItems = await _documentAdminAppService.GetFilterItemsAsync();
            filterItems.ShouldNotBeEmpty();

            filterItems.ShouldContain(p => p.ProjectId == _testData.ProjectId);
            filterItems.ShouldContain(p => p.Version == "2.0.0" && p.ProjectId == _testData.ProjectId);
            filterItems.ShouldContain(p => p.LanguageCode == "en" && p.ProjectId == _testData.ProjectId);
            filterItems.ShouldContain(p => p.Format == "md");
            
        }
    }
}
