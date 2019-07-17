using System.Threading.Tasks;
using Shouldly;
using Volo.Docs.Documents;
using Volo.Docs.GitHub.Documents;
using Volo.Docs.Projects;
using Xunit;

namespace Volo.Docs
{
    public class GithubDocumentStore_Tests : DocsDomainTestBase
    {
        private readonly IDocumentStoreFactory _documentStoreFactory;
        private readonly IProjectRepository _projectRepository;
        private readonly DocsTestData _testData;

        public GithubDocumentStore_Tests()
        {
            _documentStoreFactory = GetRequiredService<IDocumentStoreFactory>();
            _projectRepository = GetRequiredService<IProjectRepository>();
            _testData = GetRequiredService<DocsTestData>();
        }

        [Fact]
        public async Task GetDocumentAsync()
        {
            var store = _documentStoreFactory.Create(GithubDocumentStore.Type);

            var project = await _projectRepository.FindAsync(_testData.PorjectId);
            project.ShouldNotBeNull();

            var document = await store.GetDocumentAsync(project, "index2", "en", "0.123.0");
            document.ShouldNotBeNull();

            document.Title.ShouldBe("index2");
            document.FileName.ShouldBe("index2");
            document.Version.ShouldBe("0.123.0");
            document.Content.ShouldBe("stringContent");
        }

        [Fact]
        public async Task GetVersionsAsync()
        {
            var store = _documentStoreFactory.Create(GithubDocumentStore.Type);

            var project = await _projectRepository.FindAsync(_testData.PorjectId);
            project.ShouldNotBeNull();

            var document = await store.GetVersionsAsync(project);
            document.ShouldNotBeNull();

            document.Count.ShouldBe(1);
            document.ShouldContain(x => x.Name == "0.15.0" && x.DisplayName == "0.15.0");
        }

        [Fact]
        public async Task GetResource()
        {
            var store = _documentStoreFactory.Create(GithubDocumentStore.Type);

            var project = await _projectRepository.FindAsync(_testData.PorjectId);
            project.ShouldNotBeNull();

            var documentResource = await store.GetResource(project, "index.md", "en", "0.123.0");
            documentResource.ShouldNotBeNull();

            documentResource.Content.ShouldBe(new byte[]
            {
                0x01, 0x02, 0x03
            });
        }
    }
}
