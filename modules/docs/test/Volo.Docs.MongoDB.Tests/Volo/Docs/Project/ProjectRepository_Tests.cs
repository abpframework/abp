using Volo.Docs.MongoDB;
using Xunit;

namespace Volo.Docs.Project
{
    [Collection(MongoTestCollection.Name)]
    public class ProjectRepository_Tests : ProjectRepository_Tests<DocsMongoDBTestModule>
    {
    }
}
