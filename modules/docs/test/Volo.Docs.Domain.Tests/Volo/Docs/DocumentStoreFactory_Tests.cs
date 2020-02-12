using Shouldly;
using Volo.Docs.Documents;
using Volo.Docs.FileSystem.Documents;
using Volo.Docs.GitHub.Documents;
using Xunit;

namespace Volo.Docs
{
    public class DocumentStoreFactory_Tests : DocsDomainTestBase
    {
        private readonly IDocumentSourceFactory _documentStoreFactory;

        public DocumentStoreFactory_Tests()
        {
            _documentStoreFactory = GetRequiredService<IDocumentSourceFactory>();
        }

        [Fact]
        public void Create()
        {
            _documentStoreFactory.Create(GithubDocumentSource.Type).GetType().ShouldBe(typeof(GithubDocumentSource));
            _documentStoreFactory.Create(FileSystemDocumentSource.Type).GetType().ShouldBe(typeof(FileSystemDocumentSource));
        }
    }
}
