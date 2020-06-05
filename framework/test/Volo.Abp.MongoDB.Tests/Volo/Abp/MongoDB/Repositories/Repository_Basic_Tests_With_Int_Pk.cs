using System.Threading.Tasks;
using Volo.Abp.TestApp.Testing;
using Xunit;

namespace Volo.Abp.MongoDB.Repositories
{
    [Collection(MongoTestCollection.Name)]
    public class Repository_Basic_Tests_With_Int_Pk : Repository_Basic_Tests_With_Int_Pk<AbpMongoDbTestModule>
    {
        [Fact(Skip = "Int PKs are not working for MongoDb")]
        public override Task Get()
        {
            return Task.CompletedTask;
        }
    }
}