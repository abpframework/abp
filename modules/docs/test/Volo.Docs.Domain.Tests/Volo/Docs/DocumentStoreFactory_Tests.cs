using System;
using System.Collections.Generic;
using System.Text;
using Shouldly;
using Volo.Docs.Documents;
using Volo.Docs.FileSystem.Documents;
using Volo.Docs.GitHub.Documents;
using Xunit;

namespace Volo.Docs
{
    public class DocumentStoreFactory_Tests : DocsDomainTestBase
    {
        private readonly IDocumentStoreFactory _documentStoreFactory;

        public DocumentStoreFactory_Tests()
        {
            _documentStoreFactory = GetRequiredService<IDocumentStoreFactory>();
        }

        [Fact]
        public void Create()
        {
            _documentStoreFactory.Create(GithubDocumentStore.Type).GetType().ShouldBe(typeof(GithubDocumentStore));
            _documentStoreFactory.Create(FileSystemDocumentStore.Type).GetType().ShouldBe(typeof(FileSystemDocumentStore));
        }
    }
}
