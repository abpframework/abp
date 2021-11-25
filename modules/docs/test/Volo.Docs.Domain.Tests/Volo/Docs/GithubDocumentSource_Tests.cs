using System.Threading.Tasks;
using Shouldly;
using Volo.Docs.Documents;
using Volo.Docs.GitHub.Documents;
using Volo.Docs.Projects;
using Xunit;

namespace Volo.Docs
{
    public class GithubDocumentSource_Tests : DocsDomainTestBase
    {
        private readonly IDocumentSourceFactory _documentSourceFactory;
        private readonly IProjectRepository _projectRepository;
        private readonly DocsTestData _testData;

        public GithubDocumentSource_Tests()
        {
            _documentSourceFactory = GetRequiredService<IDocumentSourceFactory>();
            _projectRepository = GetRequiredService<IProjectRepository>();
            _testData = GetRequiredService<DocsTestData>();
        }

        [Fact]
        public async Task GetDocumentAsync()
        {
            var source = _documentSourceFactory.Create(GithubDocumentSource.Type);

            var project = await _projectRepository.FindAsync(_testData.PorjectId);
            project.ShouldNotBeNull();

            var document = await source.GetDocumentAsync(project, "index2", "en", "0.123.0");
            document.ShouldNotBeNull();

            document.Name.ShouldBe("index2");
            document.FileName.ShouldBe("index2");
            document.Version.ShouldBe("0.123.0");
            document.Content.ShouldBe("stringContent");
        }

        [Fact]
        public async Task GetVersionsAsync()
        {
            var source = _documentSourceFactory.Create(GithubDocumentSource.Type);

            var project = await _projectRepository.FindAsync(_testData.PorjectId);
            project.ShouldNotBeNull();

            var document = await source.GetVersionsAsync(project);
            document.ShouldNotBeNull();

            document.Count.ShouldBe(1);
            document.ShouldContain(x => x.Name == "0.15.0" && x.DisplayName == "0.15.0");
        }

        [Fact]
        public async Task GetResource()
        {
            var source = _documentSourceFactory.Create(GithubDocumentSource.Type);

            var project = await _projectRepository.FindAsync(_testData.PorjectId);
            project.ShouldNotBeNull();

            var documentResource = await source.GetResource(project, "index.md", "en", "0.123.0");
            documentResource.ShouldNotBeNull();

            documentResource.Content.ShouldBe(new byte[]
            {
                0x01, 0x02, 0x03
            });
        }
    }
}
