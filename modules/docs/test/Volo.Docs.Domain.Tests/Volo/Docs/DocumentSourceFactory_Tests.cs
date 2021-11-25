using Shouldly;
using Volo.Docs.Documents;
using Volo.Docs.FileSystem.Documents;
using Volo.Docs.GitHub.Documents;
using Xunit;

namespace Volo.Docs
{
    public class DocumentSourceFactory_Tests : DocsDomainTestBase
    {
        private readonly IDocumentSourceFactory _documentSourceFactory;

        public DocumentSourceFactory_Tests()
        {
            _documentSourceFactory = GetRequiredService<IDocumentSourceFactory>();
        }

        [Fact]
        public void Create()
        {
            _documentSourceFactory.Create(GithubDocumentSource.Type).GetType().ShouldBe(typeof(GithubDocumentSource));
            _documentSourceFactory.Create(FileSystemDocumentSource.Type).GetType().ShouldBe(typeof(FileSystemDocumentSource));
        }
    }
}
