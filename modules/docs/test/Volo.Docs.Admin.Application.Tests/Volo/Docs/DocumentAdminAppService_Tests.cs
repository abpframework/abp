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

        [Fact(Skip = "Will be PullAsync in a background job.")]
        public async Task PullAsync()
        {
            (await _documentRepository.FindAsync(_testData.PorjectId, "Part-I.md", "en", "1.0.0")).ShouldBeNull();

            await _documentAdminAppService.PullAsync(new PullDocumentInput
            {
                ProjectId = _testData.PorjectId,
                Name = "Part-I.md",
                LanguageCode = "en",
                Version = "1.0.0"
            });

            (await _documentRepository.FindAsync(_testData.PorjectId, "Part-I.md", "en", "1.0.0")).ShouldNotBeNull();
        }

        [Fact(Skip = "Will be PullAllAsync in a background job.")]
        public async Task PullAllAsync()
        {
            (await _documentRepository.FindAsync(_testData.PorjectId, "Part-I.md", "en", "1.0.0")).ShouldBeNull();
            (await _documentRepository.FindAsync(_testData.PorjectId, "Part-II.md", "en", "1.0.0")).ShouldBeNull();

            await _documentAdminAppService.PullAllAsync(new PullAllDocumentInput
            {
                ProjectId = _testData.PorjectId,
                LanguageCode = "en",
                Version = "1.0.0"
            });

            (await _documentRepository.FindAsync(_testData.PorjectId, "Part-I.md", "en", "1.0.0")).ShouldNotBeNull();
            (await _documentRepository.FindAsync(_testData.PorjectId, "Part-II.md", "en", "1.0.0")).ShouldNotBeNull();
        }
    }
}
