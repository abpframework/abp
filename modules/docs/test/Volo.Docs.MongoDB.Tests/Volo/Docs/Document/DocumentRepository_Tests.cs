using Volo.Docs.MongoDB;
using Xunit;

namespace Volo.Docs.Document
{
    [Collection(MongoTestCollection.Name)]
    public class DocumentRepository_Tests : DocumentRepository_Tests<DocsMongoDBTestModule>
    {

    }
}